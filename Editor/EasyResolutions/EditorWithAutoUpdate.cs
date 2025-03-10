using UnityEditor;
using UnityEngine.UIElements;

namespace EasyResolutions {
internal abstract class EditorWithAutoUpdate : Editor {
    const string _versionPropertyName = "_version";
    int _targetVersion;

    protected VisualElement Root { get; private set; }

    public override VisualElement CreateInspectorGUI() {
        Root = new VisualElement();
        RecreateUI();
        return Root;
    }

    void OnEnable() {
        OnSetup();
        Undo.undoRedoPerformed += OnUndoRedo;
        EditorApplication.update += RefreshOnce;
    }

    void OnDisable() {
        Undo.undoRedoPerformed -= OnUndoRedo;
        EditorApplication.update -= RefreshOnce;
        OnCleanup();
    }

    void OnUndoRedo() => RecreateUI();

    // When teh data serialized by ScriptableObject is changed from code, UI updates
    // don't trigger automatically. In most cases it is possible to trigger them
    // from Editor code. But there is no way to trigger UI updates from ScriptableObject's
    // OnEnable. Where the defaults are set. This may result in Inspector not displaying
    // the actual state of ScriptableObject. Fix: trigger a UI refresh on next Editor
    // update after Inspector is enabled. But only once, and only when non user-made
    // changes are detected.
    void RefreshOnce() {
        if (Root == null)
            return;

        serializedObject.Update();
        var targetVersion = GetVersionIfAvailable(serializedObject);
        if (_targetVersion != targetVersion)
            RecreateUI();

        EditorApplication.update -= RefreshOnce;
    }

    public void RecreateUI() {
        if (Root == null)
            return;

        Root.Clear();
        serializedObject.Update();
        _targetVersion = GetVersionIfAvailable(serializedObject);
        OnRefreshUI();
    }

    static int GetVersionIfAvailable(SerializedObject serializedObject) {
        var versionProperty = serializedObject.FindProperty(_versionPropertyName);
        return versionProperty?.intValue ?? 0;
    }

    protected virtual void OnSetup() { }
    protected abstract void OnRefreshUI();
    protected virtual void OnCleanup() { }
}
}