   M S B A M L   `   `   `  ����I  ECamemisOffLine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CamemisOffLineCamemisOffLine  5   	       CamemisOffLine.Login   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   (localclr-namespace:CamemisOffLine  5   	   -   5          � Title$  Login��5	   	   $	��600��6   $
��1080��6$    � WindowStartupLocation$ CenterScreen=�61    � WindowStyle$
 None=�6V    � WindowState$ 	Maximized=�6i   .��5
      � .+�5   
   � .+�5      �� -   5        $��backgroundImg��5   a   $1��+/CamemisOffLine;component/Image/Nature3.jpg��6   $��UniformToFill=�6I    J� Opacity$ 1Q�6v   5      �� 5   
   -   5        $��Center=�5      $��Right=�6,   $��70��6H   $��0,272,22,270q�6S   $��50��6�   ��5      �� 5       �� ImageSource$1 +/CamemisOffLine;component/Image/Nature3.jpg��6    �� Opacity$	 0.9Q�6W   5      �� 5   
   -   5        $��Center=�5      $��Right=�6,   $��70��6H   $��0,217,22,325q�6S   $��50��6i   ��5      �� 5      $1 +/CamemisOffLine;component/Image/Nature2.jpg��6   $	 0.8Q�6W   5      �� 5   
   -   5        $��Center=�5      $��0,164,22,378q�6,   $��50��6B   $��Right=�6e   $��70��6�   ��5      �� 5      $1 +/CamemisOffLine;component/Image/Nature1.jpg��6   6Y   5!      �� 5#   
   -   5        $��Center=�5#      $��Right=�6,   $��70��6H   $��0,325,22,217q�6S   $��50��6�   ��5$      �� 5%      $1 +/CamemisOffLine;component/Image/Nature4.jpg��6   $	 0.8Q�6W   5(      �� 5)   
   -   5        $��Center=�5)      $��Right=�6,   $��70��6H   $��0,378,22,164q�6S   $��50��6�   ��5*      �� 5+      -   5        	back5��$1 +/CamemisOffLine;component/Image/Nature5.jpg��5+   ,   $	 0.8Q�6g   5.      � 51   
   -	   5        $��	gridLogin��51      $	��380��6    $	��450��6,   ��53      �� 54       �� EndPoint$ 0.5,1&�6&    � MappingMode$ RelativeToBoundingBox=�67   	 �� 
StartPoint$	 0.5,0&�6[   $	 0.8Q�6n   .��55      � 
 � Color$
 	#FF1ECAE6��6#   � 56      $
 	#FF91A257��6#    � Offset$ 0.751Q�65   57      .+�59      �� .+�5:      �� $1��+/CamemisOffLine;component/Image/unnamed.png��6   $��53��6M   $��30q�6Y    J� OpacityMask 5;      �� 5<       �� Color$ Black��6*   $	 0.5Q�68   5>      �� 5@      $��CAMEMIS��6   $��59��6+    J� RenderTransformOrigin$ 0.506,-0.43&�67   
�������$��Center=�6n   $��18(�6�   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   $��126,-60q�6�   �� 5B      -
   5        $��btnPassCode��5B      $��0��60   .+�5C      ��  �� Orientation$ 
Horizontal=�6!   $��74,10,26,10q�6:    � ForceCursor . .+�5D      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ PasswordReset=�62   $��25��6G   
�������$��26��6d   ���	#FFCAE8DE$	��0 9q�6�   �� 5E      -   5        $��txtCode��5E   �   
�������$��2q�65   $��3,0,0,0q�6I   ���	#FFCAE8DE)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ School Code��6r   
���   �$	��204��6�   $��20)�6�   $��Center=�6�    . 6  5G      5I      �� 5K      -   5        $��btnUser��5K      $��1��6,   .+�5L      �� $ 
Horizontal=�6!   $��74,17,26,17q�6:   .+�5M        $
 User=�62   $��25��6>   
�������$��26��6[   ���	#FFCAE8DE$	��0 0q�6~   �� 5N      -   5        $��txtUser��5N   "   
�������$��2q�6D   $��3,0,0,0q�6X   $	��0 0q�6j   $ Enter Username/Email��6w   ���	#FFCAE8DE
���   �$	��204��6�   $��20)�6�   6  5P      5Q      �� 5S      -   5        $��btnPass��5S      $��1��6,   .+�5T      �� $ 
Horizontal=�6!   $��74,17,26,17q�6:   .+�5U        $
 Lock=�62   
�������$��25��6P   $��26��6[   ���	#FFCAE8DE$	��0 0q�6~   G� 5V      -   5        $��txtPass��5V   �   $	��0 0q�6'   
�������$��2q�6G   $��3,0,0,0q�6[   ���	#FFCAE8DE$ Password��6�   
���   �$	��204��6�   $��20)�6�   6�   5X      5Y      �� 5Z      $
��50,5q�6   .+�5[      �� -   5        $��lblError��5[      
���  ��$��10(�6=   $��10q�6K   $$��Incorrect Username or Password��6W   $��Center=�6�   $��Center=�6�   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   6  �� 5\      -   5        $��cbRememberMe��5\      $��
Remeber me��63   $��15(�6H   $��Center=�6V   
�������$>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   6�   5]      �� 5_      $ 
Horizontal=�6   .+�5`      �� -   5        $��btnLogin��5`      $��40��6D   
������ $��0q�6�   $��
140,20,0,0q�6�    �� 	IsDefault . .��5a      � $��19��6   $��62��6+    � ColumnDefinitions 5b      �� 5c      $	��12* �6/   �� 5d      $	��31* �6/   .+�5i      � $��0��6   .+�5j      � $��0,-10,0,-11q�6#   .+�5k   "     $ Login=�6:   $��27��6G   $��17��6S   $��0,3,0,0q�6^   6q   5l      5m      � 5n      $��1��6   $��	0,0,-20,0q�6/   .+�5o   "   �� $��Sign in��6(   
�������$��15(�6M   $��
0,-8,-14,0q�6[   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6p   6�   5p      5q      5s      �� 5t      -   5        $��btnNext��5t      $��40��6,   
������ $��0q�6�    . .��5u      � $��19��6   $��62��6+    5v      �� 5w   "   $	��12* �63   �� 5x   "   $	��31* �63   .+�5}      � $��0��6#   .+�5~   "   � $��0,-10,0,-11q�6'   .+�5   &     $ SkipNext=�6>   $��27��6N   $��17��6Z   $��0,3,0,0q�6e   6x   5�   #   5�      � 5�      $��1��6#   $��	0,0,-20,0q�63   .+�5�   "   �� $
��Next��6(   
�������$��15(�6J   $��
0,-8,-14,0q�6X   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6m   6�   5�      5�      5�      5�      5�      � 5�      -   5        $��	grideExit��5�      $	��Top=�6i   $��Right=�6�   .+�5�        -   5        $ Close=�5�   *    � Cursor$
 Hand��67   $��21��6d   $��22��6p   6~   5�      5�      5�      5�      