Skeletal Viewer - READ ME 

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the terms of the Microsoft Kinect for
Windows SDK (Beta) from Microsoft Research License Agreement:
http://research.microsoft.com/KinectSDK-ToU


=============================
OVERVIEW  
.............................
This module provides sample code used to demonstrate Kinect NUI processing such as
capturing depth stream, color stream and skeletal tracking frames and displaying them
on the screen.
When sample is executed you should be able to see the following:
- the depth stream, showing background in grayscale and different people in different
  colors, darker colors meaning farther distance from camera. Note that people will
  only be detected if their entire body fits within captured frame.
- Tracked NUI skeletons of people detected within frame. Note that NUI skeletons will
  only appear if the entire body of at least one person fits within captured frame.
- Color video stream
- Frame rate at which capture is being delivered to sample application.

=============================
SAMPLE LANGUAGE IMPLEMENTATIONS     
.............................
This sample is available in C#


=============================
FILES   
.............................
- App.xaml: declaration of application level resources
- App.xaml.cs: interaction logic behind app.xaml
- MainWindow.xaml: declaration of layout within main application window
- MainWindow.xaml.cs: NUI initialization, processing and display code
- SkeletalViewer.ico: Application icon used in title bar and task bar

=============================
BUILDING THE SAMPLE   
.............................

To build the sample using Visual Studio:
-----------------------------------------------------------
1. In Windows Explorer, navigate to the SkeletalViewer\CS directory.
2. Double-click the icon for the .sln (solution) file to open the file in Visual Studio.
3. In the Build menu, select Build Solution. The application will be built in the default \Debug or \Release directory.


=============================
RUNNING THE SAMPLE   
.............................

To run the sample:
------------------
1. Navigate to the directory that contains the new executable, using the command prompt or Windows Explorer.
2. Type SkeletalViewer at the command line, or double-click the icon for SkeletalViewer.exe to launch it from Windows Explorer.

