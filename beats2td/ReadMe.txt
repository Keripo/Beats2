Last updated: 2012/12/27
~Keripo

This is the source code for Beats2. For more information, please see the following websites:
http://beatsportable.com/
http://code.google.com/p/beats2/

The project was created with Unity 4.0.0f7 on Windows 8 (x64) using "2D Toolkit v1.80 final + patch 3"
The "2D Toolkit" (referred to as "tk2d") package is not included - you will have to obtain a license for it from:
http://www.unikronsoftware.com/2dtoolkit/

To import the project:
1) Run Unity's "Project Wizard" (File -> Open Project...)
2) Click "Open Other..." and select the "beats2" folder
3) Open the "Asset Store" (Window -> Asset Store)
4) Download and import the "2D Toolkit" asset (you do not need to import "TK2DROOT/tk2d_demo")
4.5) tk2d bug workaround: Copy the file "Assets/TK2DROOT/tk2d/Shaders/BlendVertexColor.shader" to your "Assets/Resources" folder
5) Click "Clear" on the "Console" window and check for errors
6) Click the Run icon in the Unity Editor to make sure the game runs
7) Open up the "Build Settings" (File -> Build Settings...
8) Click on "Player Settings..." and update the Cross-Platform settings accordingly
9) Click on the "Build Settings" "Build" button
10) Name and save the binary output file somewhere (such as the root of the "beats2" folder)
11) Copy the "beats2/Beats2" folder to the same folder you saved the binary in (you don't have to do this if you saved it in the root "beats2" folder)
12) Run the binary app

Please not that not all files of this project are under the same license. The C# scripts in the "Assets/Scripts" folder, however, are all under the Modified BSD license - see "Assets/Scripts/License.txt"
