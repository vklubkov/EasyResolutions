Easy Resolutions
===
![Easy Resolutions logo](https://cloud.githubusercontent.com/assets/7109579/6622120/a3575c22-c8eb-11e4-80bf-6a8c61a33a18.png?raw=true "Easy Resolutions")

[Download GitHub release](https://github.com/marked-one/EasyResolutions/releases/latest)  
[Download from Unity Asset Store](http://u3d.as/content/vladimir-klubkov/easy-resolutions)

Easy Resolutions is an asset  for [Unity3d](http://unity3d.com/) (now supports Unity 5) which allows you to automatically load different scenes for different resolutions of the game window.

It is especially suitable for 2d games when you need to support a large amount of devices with different screen resolutions/aspect ratios/amount of video memory.

The asset is easy to use:
1. First you name your scenes using the following template: `{scene name}.{width}x{height}`, where the `{width}x{height}` part is the resolution, for which the scene is designed. Example: `SomeScene.1366x768`. 
2. Then, in the Editor, you generate the Easy Resolutions XML file using the `Assets->Create->Easy Resolutions XML file` menu. The file appears in the `EasyResolutions/Resources` folder. Only scenes added to the `Build Settings` and marked as enabled are present in it.
3. Finally, you load the scenes in your scripts using the following code (please note that the asset is written in C#!):
  
        Application.LoadLevel("SceneName".PickResolution());
The `PickResolution` method does all the job of choosing the best scene resolution for you.


## Import

When you import the package, you may import it partially. 
* `Scripts` folder contains the C# scripts. You absolutely need this folder in order to use the asset in your project. 
* `Example` folder contains a simple usage example. You only need it if you want to see the example.
* `README.txt` file contains a plain text version of this description. You only need it in your project if you want to read this description.


## Example

1. Be sure that you have the `EasyResolutions/Example` and the `EasyResolutions/Scripts` folders imported.
2. Add scenes from the `EasyResolutions/Example/Scenes` folder to the `Build Settings` (`File->Build Settings...`).
3. Open the `RunThisScene` scene in the Editor and run it.
4. Try enabling and disabling (you can do it in the `Build Settings`) the scenes, which names start with the `TestScene` and see the result.
5. Open the `EasyResolutions/Example/Scripts/First.cs` script to see the code.
6. Run the `Assets->Create->Easy Resolutions XML file` menu item, create a build for your OS and run the example outside of the Editor.


## Usage

Let's imagine that you are starting a new empty project. In this case, you could do the following:
1. Import `EasyResolutions/Scripts` folder (and all of its contents) to your project.
2. Create a scene named `First`. This scene will be the first scene in your project.
3. Create a bunch of scenes with the same name but different resolutions. Let's imagine that you need your main menu to be different for iPhone5 and iPad. In this case you need to create at least two scenes: `MainMenu.1136x640` and `MainMenu.1024x768`.
4. Create a C# script named `First` and add it as a component to the camera (or you may create any other object) in the scene named `First`. Change the contents of the script to:

        using EasyResolutions;
        using UnityEngine;
        
        public class First : MonoBehaviour
        {
            private void Start()
            {
                Application.LoadLevel("MainMenu".PickResolution());
            }
        }
5. Add all the scenes to `Build Settings`. The scene named `First` should be the first scene in the list.
6. Now you may want to add some sprites to your scenes, to see if `PickResolution` is doing the right thing for you. 
7. Run the scene named `First` and see how it works.
8. You may also want to make a build and run it outside of the editor. Don't forget to run `Assets->Create->Easy Resolutions XML file` before creating a build. This is a necessary step because Unity does not have a feature to run scripts just before a build.


## How it works

Easy Resolutions project is a bit tricky inside, but, in short, it works this way:
1. When a scene runs in the Editor or when you manually run `Assets->Create->Easy Resolutions XML file`, the scripts programmatically extract all the enabled scenes from `Build Settings` and save them to an XML file (named `easyresolutions.xml` by default) in the `EasyResolutions/Resources` folder.
2. When `PickResolution` is invoked for the first time during playing, this file is loaded into memory and parsed.
3. After the file is read and parsed, `PickResolution` checks resolutions available for the specified scene against the game window resolution and chooses the best one with the use of some cunning algorithm. The `PickResolution` method returns the full name of the scene and`Application.LoadLevel()` becomes it as an argument.


## Customization

The asset is highly customizable, but its customization requires some programming skills. Look at the examples:

1. `PickResolution` parameters

        using EasyResolutions;
        using UnityEngine;
        
        public class First : MonoBehaviour
        {
            private void Start()
            {
                // 1. With SceneMode.Fill specified, the algorithm 
                // tries to find a scene with the smallest possible aspect ratio 
                // that completely covers the game window. Thus, some 
                // parts of the scene may be cropped. SceneMode.Fill
                // is used by default.
                Application.LoadLevel("MainMenu".PickResolution());
                Application.LoadLevel("MainMenu".PickResolution(SceneMode.Fill));
                
                // 2. With SceneMode.Fit specified, the algorithm tries to 
                // find the scene with the greatest possible aspect ratio, that 
                // completely fits into the game window. Thus, some
                // parts of the game window may be not covered by the scene. 
                Application.LoadLevel("MainMenu".PickResolution(SceneMode.Fit));
                
                // 3. You may specify your own resolution to load.
                // The current resolution of the game window is used by default.
                Application.LoadLevel("MainMenu".PickResolution(1024, 768));
                
                // 4. You may specify both SceneMode and a custom resolution.
                Application.LoadLevel("MainMenu".PickResolution(1024, 768, SceneMode.Fit));
                Application.LoadLevel("MainMenu".PickResolution(1024, 768, SceneMode.Fill));
            }
        }
2. Load from your own XML

        using EasyResolutions;
        using UnityEngine;
        
        public class First : MonoBehaviour
        {
            private void Start()
            {
                // 1. If you want to load the data from a 
                // custom XML file in the Resources folder, 
                // just set its name to XmlFile.
                XmlFile.FileName = "your.xml";
                Application.LoadLevel("MainMenu".PickResolution());
                
                // 2. You may also load scenes and resolutions
                // from an XElement.
                XElement root; // Your XElement.
                var available = new AvailableResolutions(); 
                available.FromXml(root);
                SceneResolutionPicker.AvailableResolutions = available;
                Application.LoadLevel("MainMenu".PickResolution());
            }
        }
3. Add scenes at runtime

        using EasyResolutions;
        using UnityEngine;
        
        public class First : MonoBehaviour
        {
            private void Start()
            {
                // You may add scenes at runtime:
                var available = new AvailableResolutions(); 
                available.Add("YourScene.640x480");
                available.Add("YourScene", 1440, 1440);
                SceneResolutionPicker.AvailableResolutions = available;
                Application.LoadLevel("MainMenu".PickResolution());
            }
        }
4. Tweaking the comparison (requires strong programming skills)
        
        using EasyResolutions;
        using UnityEngine;
        
        public class First : MonoBehaviour
        {
            private void Start()
            {
                // You may create the available resolutions object
                // explicitly and then customize it further...
                var available = new AvailableResolutions(); 
                // ... or you may implement your own available resolutions 
                // class by deriving from the IAvailableResolutions interface.
              
                // You may create the resolution comparer object
                // explicitly and then customize it further...
                var comparer = new ResolutionComparer();
                // ...or you may implement your own resolution comparer 
                // by deriving from the IResolutionComparer interface.
                
                // You may customize the comparer using different combinations
                // of existing comparison components...
                var sizeComparisonComponent = new SizeResolutionComparisonComponent();
                var nullComparisonComponent = new NullResolutionComparisonComponent();
                // ...or you may implement your own comparison components
                // by deriving from IResolutionComparisonComponent interface.
                
                // You may also customize the SizeResolutionComparisonComponent
                // through implementing your own single dimension comparer
                // classes by deriving from ISingleDimensionComparer interface.
                sizeComparisonComponent.WidthComparer = new YourSingleDimensionComparer();
                
                // You may customize the order of comparison components.
                comparer.AppendComparisonComponent(nullComparisonComponent);
                comparer.AppendComparisonComponent(sizeComparisonComponent);
                
                // You may add the new comparer to AvailableResolutions as FitComparer 
                // or FillComparer, or as both.
                available.FitComparer = comparer;
                available.FillComparer = comparer;
                
                // Add the new AvailableResolutions object to scene resolution picker.
                SceneResolutionPicker.AvailableResolutions = available;
                
                // And finally do the trick.
                Application.LoadLevel("MainMenu".PickResolution());
            }
        }


## Tips

* Don't call the `PickResolution` method from `Awake` event function, because when running from Editor `Awake` is invoked before the XML file is read!
* If you create a scene without adding a resolution to its name, this scene has higher priority than the scenes of same name but with resolutions. This may be useful during the development process, but be careful in release builds!
* The scene itself may have any resolution you want. But if you need to have a 2d scene, which background has exactly the same resolution as specified in the scene name, you will need to tune the `Size` of the orthographic camera and the `Pixels To Units` value of all textures used in this scene.
* Log in Easy Resolutions is automatically disabled, when not in debug mode. If you want to switch it off for debug too, use `EasyResolutions.Log.Enabled = false;` somewhere in your scripts.
* When you run `Assets->Create->Easy Resolutions XML file`, the file is always created in the `EasyResolutions/Resources` folder.


## Licensing

The asset is dual-licensed: 
* the Asset Store version is under the [Asset Store EULA](http://unity3d.com/legal/as_terms),
* the GitHub version is under MIT/X11 license, its text follows:

        Copyright © 2014 Vladimir Klubkov
    
        Permission is hereby granted, free of charge, to any person obtaining a copy of this software
        and associated documentation files (the "Software"), to deal in the Software without
        restriction, including without limitation the rights to use, copy, modify, merge, publish,
        distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
        Software is furnished to do so, subject to the following conditions:
        The above copyright notice and this permission notice shall be included in all copies or
        substantial portions of the Software.
    
        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
        BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
        DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
