Easy Resolutions
===

[Asset Store Link](http://u3d.as/content/vladimir-klubkov/easy-resolutions)

![Easy Resolutions logo](https://cloud.githubusercontent.com/assets/7109579/4121004/28abcbb8-32b7-11e4-890a-9df3357476df.png?raw=true "Easy Resolutions")

Easy Resolutions is a bunch of C# scripts for [Unity3d](http://unity3d.com/), which allow you to load different versions of same scene for different screen resolutions. The scene is selected automatically depending on the current resolution of the game window.

This is especially suitable for 2d games when you need to support a large amount of devices with different screen resolutions/aspect ratios and with different amount of video memory available.

The usage is simple: you name your scenes using the following template: `{scene name}.{width}x{height}`, where the `{width}x{height}` part is the resolution, for which this scene is designed. Example: `SomeScene.1366x768`. 

Then, in the Editor, you generate Easy Resolutions XML file from the menu: `Assets->Create->Easy Resolutions XML file`. The file appears in `EasyResolutions/Resources` folder. Only scenes added to the `Build Settings` and marked as enabled are present in it.

Finally, in your scripts you load scenes using the following code: 	
  
        Application.LoadLevel("SceneName".PickResolution());
The `PickResolution` method does all the job of selecting the best scene resolution for you.


## Reasons

This approach allows you:
* To support screens with different aspect ratios: for example, you may have a wider version of a scene for 16:9 aspect ratio, and another version of the same scene for 4:3 aspect ratio.
* To load two different scenes for different screen resolutions, and even to load different amount of scenes for different screen resolutions! For example, for a screen with larger resolution you may have both main menu and levels menu in a single scene, and for a screen with smaller resolution you may have two different scenes for main menu and levels menu.
* To have more detailed textures for high-resolution devices and less detailed textures for low-resolution devices.


## Importing the package

When you import the package, you can import it partially. This section describes, what you definitely should import, and what you could throw away.
* `Scripts` - folder containing C# scripts. You absolutely need this folder in order to use Easy Resolutions in your project. 
* `Example` - folder containing a simple usage example. You don't need it in your project. A good idea is to create a separate project to run the example.
* `README.txt` - file, containing plain text version of this description. You don't need it in your project.


## Running the example

1. Be sure, you have imported `EasyResolutions/Example` and `EasyResolutions/Scripts` folder and all of their contents to the project.
2. Add scenes from `EasyResolutions/Example/Scenes` folder to the `Build Settings` (`File->Build Settings...`).
3. Open the `RunThisScene` scene in the Editor and run it.
4. Try enabling and disabling (you can do it in the `Build Settings`) scenes, which names start with `TestScene`, and see the results.
5. Open `EasyResolutions/Example/Scripts/First.cs` script to see the code.
6. Run `Assets->Create->Easy Resolutions XML file` from the menu, create a build for your OS and run the example outside of the Editor.


## Usage

Let's imagine that you are starting a new empty project. In this case, you could do the following:
1. Import `EasyResolutions/Scripts` folder (and all of its contents) to your project.
2. Create the scene named `First`. This scene will be the first scene in your project.
3. Create a bunch of scenes with different resolutions. Let's imagine, that you need your main menu to be different for iPhone5 and iPad. In this case you need to create at least two scenes: `MainMenu.1136x640` and `MainMenu.1024x768`.
4. Create a C# script named `First` and add it as a component to the camera in the scene named `First`. Change the contents of the script to:

        using EasyResolutions;
        using UnityEngine;
        
        public class First : MonoBehaviour
        {
            private void Start()
            {
                Application.LoadLevel("MainMenu".PickResolution());
            }
        }
5. Add all created scenes to `Build Settings`. The `First` scene should be the first in the list.
6. Now you may want to add some sprites to your scenes, to see if `PickResolution` is doing the right thing for you.
7. Run the scene named `First` and see, how it works.
8. You may also want to make a build and run it outside of the editor. Don't forget to run `Assets->Create->Easy Resolutions XML file` before creating a build. 


## How this stuff works

Easy Resolutions project is a bit tricky inside, but, in short, it works this way:
1. When the scene runs in the Editor or when you manually run `Assets->Create->Easy Resolutions XML file`, the scripts programmatically extract all the enabled scenes from `Build Settings` and save them to an XML file (named `easyresolutions.xml` by default) in `EasyResolutions/Resources` folder.
2. When `PickResolution` is invoked for the first time, this file is loaded into memory and parsed.
3. After the file is read and parsed, `PickResolution` checks resolutions available for the specified scene against game window resolution using some cunning algorithm. The best resolution is chosen, and the full scene name is returned by the `PickResolution` method.


## Customization

Easy Resolutions is highly customizable even without changing a single line of its code. Look at the examples with explanations:

1. `PickResolution` parameters

        using EasyResolutions;
        using UnityEngine;
        
        public class First : MonoBehaviour
        {
            private void Start()
            {
                // 1. With SceneMode.Fill specified, the algorithm 
                // tries to find the scene with the smallest aspect ratio, 
                // which completely covers the game window. Thus, some 
                // parts of the scene may be cropped. SceneMode.Fill
                // is the default value.
                Application.LoadLevel("MainMenu".PickResolution());
                Application.LoadLevel("MainMenu".PickResolution(SceneMode.Fill));
                
                // 2. With SceneMode.Fit specified, the algorithm tries to 
                // find the scene with the greatest aspect ratio, which 
                // completely fits into the game window. Thus, some
                // parts of the game window may be not covered by the scene. 
                Application.LoadLevel("MainMenu".PickResolution(SceneMode.Fit));
                
                // 3. You may specify your own "game resolution".
                // By default, the current resolution of the game window is used.
                Application.LoadLevel("MainMenu".PickResolution(1024, 768));
                
                // 4. You may specify SceneMode along with a custom game resolution.
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
                // 1. If you want to load from a custom 
                // XML file in Resources folder, just 
                // set its name to XmlFile.
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
3. Add available scenes at runtime

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
4. Tweaking the comparison
        
        using EasyResolutions;
        using UnityEngine;
        
        public class First : MonoBehaviour
        {
            private void Start()
            {
                // You may create the available resolutions object
                // explicitly and then customize it  further...
                var available = new AvailableResolutions(); 
                // ... or you may implement your own available resolutions 
                // class by deriving from IAvailableResolutions interface.
              
                // You may create the resolution comparer object
                // explicitly and then customize it further...
                var comparer = new ResolutionComparer();
                // ...or you may implement your own resolution comparer 
                // by deriving from IResolutionComparer interface.
                
                // You may customize the comparer using different combinations
                // of existing comparison components...
                var sizeComparisonComponent = new SizeResolutionComparisonComponent();
                var nullComparisonComponent = new NullResolutionComparisonComponent();
                // ...or you may implement your own comparison components
                // by deriving from IResolutionComparisonComponent interface.
                
                // You also may customize the SizeResolutionComparisonComponent
                // through implementing your own single dimension comparer
                // classes by deriving from ISingleDimensionComparer interface.
                sizeComparisonComponent.WidthComparer = new YourSingleDimensionComparer();
                
                // You may customize the order of comparison components.
                comparer.AppendComparisonComponent(nullComparisonComponent);
                comparer.AppendComparisonComponent(sizeComparisonComponent);
                
                // Add the new comparer to AvailableResolutions as FitComparer 
                // or FillComparer, or as both.
                available.FitComparer = comparer;
                available.FillComparer = comparer;
                
                // Add new AvailableResolutions object to scene resolution picker.
                SceneResolutionPicker.AvailableResolutions = available;
                
				// Do the trick.
                Application.LoadLevel("MainMenu".PickResolution());
            }
        }


## Tips

* Don't call `PickResolution` method from `Awake`, because when running from Editor `Awake` is invoked before the XML file is read. 
* If you create a scene without adding a resolution to its name, this scene has higher priority than the scenes of same name but with resolutions. This may be useful during the development process, but be careful in release builds.
* The scene itself may have any resolution you want. But if you need to have a 2d scene, which background has exactly the same resolution as specified in the scene name, you will need to tune the `Size` of the orthographic camera and the `Pixels To Units` value of textures used in this scene.
* Logging in Easy Resolutions is automatically disabled, when not in debug. If you want to switch it off for debug, use `EasyResolutions.Log.Enabled = false;`.
* When you run `Assets->Create->Easy Resolutions XML file`, the file is always created in the `EasyResolutions/Resources` folder.


## Unity, MonoDevelop, C# and .NET versions

Scripts were created using:
* Unity 4.5.2f1
* MonoDevelop 4.0.1 with Target Framework set to Mono/.NET 3.5 and C# language version set to: Default.

## License

MIT/X11

        Copyright (C) 2014 Vladimir Klubkov
    
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
