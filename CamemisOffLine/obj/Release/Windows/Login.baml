   M S B A M L   `   `   `  ���� I  ECamemisOffLine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CamemisOffLineCamemisOffLine      CamemisOffLine.Login   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  (localclr-namespace:CamemisOffLine  -     � Title$  Login��$	��600��$
��1080�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=� � WindowState$ 	Maximized=�.��� .+�� .+��� -   $��backgroundImg��$1��+/CamemisOffLine;component/Image/Nature3.jpg��$��UniformToFill=� J� Opacity$ 1Q��� -   $��Center=�$��Right=�$��70��$��0,272,22,270q�$��50������  �� ImageSource$1 +/CamemisOffLine;component/Image/Nature3.jpg�� �� Opacity$	 0.9Q��� -   $��Center=�$��Right=�$��70��$��0,217,22,325q�$��50������ $1 +/CamemisOffLine;component/Image/Nature2.jpg��$	 0.8Q��� -   $��Center=�$��0,164,22,378q�$��50��$��Right=�$��70������ $1 +/CamemisOffLine;component/Image/Nature1.jpg���� -   $��Center=�$��Right=�$��70��$��0,325,22,217q�$��50������ $1 +/CamemisOffLine;component/Image/Nature4.jpg��$	 0.8Q��� -   $��Center=�$��Right=�$��70��$��0,378,22,164q�$��50������ -   	back5��$1 +/CamemisOffLine;component/Image/Nature5.jpg��$	 0.8Q�� -	   $��	gridLogin��$	��380��$	��450������  �� EndPoint$ 0.5,1&� � MappingMode$ RelativeToBoundingBox=�	 �� 
StartPoint$	 0.5,0&�$	 0.8Q�.��� 
 � Color$
 	#FF1ECAE6��� $
 	#FF91A257�� � Offset$ 0.751Q�.+��� .+��� $1��+/CamemisOffLine;component/Image/unnamed.png��$��53��$��30q� J� OpacityMask ��  �� Color$ Black��$	 0.5Q��� $��CAMEMIS��$��59�� J� RenderTransformOrigin$ 0.506,-0.43&�
�������$��Center=�$��18(�$��Khmer OS Siemreap)�$��126,-60q��� -
   $��btnPassCode��$��0��.+���  �� Orientation$ 
Horizontal=�$��50,10q� � ForceCursor . .+�'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ PasswordReset=�$��25��$��25��
�������$	��0 9q��� -   $��txtCode��$	��0 0q�)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ School Code��
�������$	��204��$��20)�$��Center=� . �� -   $��btnUser��$��1��.+��� $ 
Horizontal=�$��50,17q�.+�  $
 User=�$��25��$��25��
�������$	��0 0q��� -   $��txtUser��$	��0 0q�$ Enter Username/Email��
�������$	��204��$��20)��� -   $��btnPass��$��1��.+��� $ 
Horizontal=�$��50,17q�.+�  $
 Lock=�$��25��$��25��
�������$	��0 0q�G� -   $��txtPass��$	��0 0q�$ Password��
�������$	��204��$��20)��� $
��50,5q�.+��� -   $��lblError��
���  ��$��10(�$��10q�$$��Incorrect Username or Password��$��Center=�$��Center=��� -   $��cbRememberMe��$��
Remeber me��$��15(�$��Center=�
��������� $ 
Horizontal=�.+��� -   $��btnLogin��$��40��
������ $��0q�$��
140,20,0,0q� �� 	IsDefault . .��� $��19��$��62�� � ColumnDefinitions �� $	��12* ��� $	��31* �.+�� $��0��.+�� $��0,-10,0,-11q�.+�  $ Login=�$��27��$��17��$��0,3,0,0q�� $��1��$��	0,0,-20,0q�.+��� $��Sign in��
�������$��15(�$��
0,-8,-14,0q��� -   $��btnNext��$��40��
������ $��0q� . .��� $��19��$��62�� �� $	��12* ��� $	��31* �.+�� $��0��.+�� $��0,-10,0,-11q�.+�  $ SkipNext=�$��27��$��17��$��0,3,0,0q�� $��1��$��	0,0,-20,0q�.+��� $
��Next��
�������$��15(�$��
0,-8,-14,0q�� -   $��	grideExit��$	��Top=�$��Right=�.+�  -   $ Close=� � Cursor$
 Hand��$��21��$��22��