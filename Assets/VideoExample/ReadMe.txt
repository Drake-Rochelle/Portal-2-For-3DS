This example demonstrates how to play back video movies, but does not ship with video content itself.

For instructions on how to create your own correctly encoded 2D/3D videos, please refer to the Mobiclip SDK which can be obtained from Nintendo's developer portal.

https://developer.nintendo.com/group/development/wtc6ppr2/downloads
"Mobiclip for 3DS 2.0.4 (20150518)"

That package also contains a number of sample movies which can be plugged into this demo.  Please copy/rename the sample videos as follows, or modify "VideoTest.cs" as required.

Assets
 |
 +-- StreamingAssets
 |    |
 |    +-- UpperScreen.moflex			// 400x240 3D video to display on the upper screen.
 |    |
 |    +-- LowerScreen.moflex			// 320x240 2D video to display on the upper screen.
 |
 |
 +-- VideoExample
      |
      +-- ...


For example, "macroshow_imaadpcm.moflex" is suitable for use as "UpperScreen.moflex", and "motocross_imaadpcm.moflex" is suitable for use as "LowerScreen.moflex".

Finally, you will need to ensure that "Player Settings -> Other Settings -> Optional Features -> Mobiclip (video player)" is set to either "fast" or "small", as desired.

Thanks!
