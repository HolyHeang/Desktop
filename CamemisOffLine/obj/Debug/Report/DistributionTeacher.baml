   M S B A M L   `   `   `  ����I  ECamemisOffLine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:CamemisOffLine.ReportCamemisOffLine.Report  5   	   /    )CamemisOffLine.Report.DistributionTeacher   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:CamemisOffLine.Report     5   	   -   5          � Title$  DistributionTeacher��5   	   $
��32cm��6%   $
��21cm��63    � WindowState$ Normal=�6W    � 
ResizeMode$ NoResize=�6l   $
��30cm��6�    � WindowStartupLocation$ Manual=�6�   .��5	      � .+�5
   
   �� -   5        $
��Grid��5
   ,    �� Orientation$ Vertical=�6   $��Center=�68   .+�5      � -   5        $��Header��5      $��0 20 0 0q�6!    � ColumnDefinitions 5      �� 5      $��* �6'   63   �� 5      $��* �6'   63   �� 5      $��* �6'   63   .+�5      � $��0��6   .+�5      � $��0,0,0,0q�6   .+�5      �� $ Vertical=�6%   $��0,25,0,0q�6<   .+�5      �� $c��]មន្ទីរអប់រំ យុវជន និងកីឡា រាជធានី��6$      Muol#���   6P   $��14(�6o   $
��Left=�6}   $��Bottom=�6�   $��0 15 0 0q�6�   6�   � 5      $��	0,-10,0,0q�6#   .+�5   "   �� $��ភ្នំពេញ��6(   #���   6:   $��13(�6Y   
���   �$��Center=�6z   $��0 0 0 0q�6�   $��Center=�6�   6�   5      � 5      $��	0,-10,0,0q�6#   .+�5   "   �� -   5        $��TitleSchool��5   (     Siemreap#���  6;   $��13(�6^   
���   �$
��Left=�6   $��0 0 0 0q�6�   $��Center=�6�   6�   5      � 5      $��	0,-12,0,0q�6#   5      5      5      � 5      $��1��6   .+�5       � .+�5!      �� $ Vertical=�6%   $��1��6<   .+�5"      � $��0 -5 0 0q�6#   .+�5#   "   �� $E��?ព្រះរាជាណាចក្រកម្ពុជា��6(   $��Center=�6H   #���   6e   $��15(�6�   $��0 -0 0 0q�6�   6�   5$      � 5%      $��0 -8 0 0q�6#   .+�5&   "   �� $M��Gជាតិ​ សាសនា ព្រះមហាក្សត្រ��6(   $��Center=�6L   #���   6i   $��15(�6�   $��0 -5 0 0q�6�   6�   5'      �� 5(      $ 
Horizontal=�6)   $��Center=�6B   $��0 50 0 0q�6_   .+�5)   "   �� -   5        $��lblMonth��5)   ,   $i��cបញ្ជីបំណែងចែកគ្រូទទួលបន្ទុកថ្នាក់��6<   $��Center=�6e   #���   6�   $��15(�6�   
���   �$��0 0 0 0q�6�   6�   5*      5+      5,      5-      5.      � 5/      $��Center=�6   $��Center=�60   $��0 10 0 0q�6K   .+�50      �� $ 
Horizontal=�6   .+�51      �� $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6   $��15(�6b   $'��!ឆ្នាំសិក្សា��6p   
�������6�   �� 52      $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6   $��15(�6b   $��	2020-2021��6p   
�������6�   53      55      � 57      -   5        $
��Body��57      $��Center=�6   $
��21cm��6<   $��0 5 0 0q�6L    58      �� 59      $��* �6'   63   .+�5;      � $��0��6   .+�5<      � .+�5=      &   System.Windows.Controls.DataGrid  $��	10 0 10 0q�6#      AutoGenerateColumns .     	RowHeight$ 40Q�6R   $��1 1 1 0q�6a   
���   �
�������$��13(�6�   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   $
��21cm��6�      CanUserAddRows .  	 � 	Resources	 5>      
 �� 
TargetType �� Property �� Value=  7System.Windows.Controls.Primitives.DataGridColumnHeader:  4System.Windows.Controls.Primitives.DataGridRowHeader �� BasedOn��%�  '        '  �     '  �    7 �� 5@   "   "
  6(   .�5A   &   ��  � ��
 ���� 6C   �� 5B   &    � ��
 �����6D   �� 5C   &    � ��$ 0q�6H   5D   #   �� 5F   "   "
  6(   .�5G   &   ��  � ��
 �����6C   �� 5H   &    � ��
 �   �6D   �� 5I   &    � ��$ 0 0 1 1q�6H   �� 5J   &    � ��$ Center=�6S   �� 5K   &    � ��$ 40��6?   �� 5L   &    � ��$> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6C   5M   #   �� 5N   "   8   "
  6^   .�5O   &   ��  � ��
 ���� 6C   �� 5P   &    � ��$ 0��6>   �� 5Q   &    � ��$ 0��6?   6K   5R   #   5S         Columns 5U      0  *System.Windows.Controls.DataGridTextColumn  5W   "   ,  &System.Windows.Controls.DataGridColumn   Header$ 	លេខ��65      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.5* 6B      MinWidth$ 50Q�6O      FontSize$ 15Q�6]   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle 5X   &   �� 5Y   *   "
 ��60   .�5Z   .   ��  � ��
 �   �6[   �� 5[   .    � ��$
 Thin"�6K   6Z   �� 5\   .    � ��$ Center=�6T   �� 5]   .    � ��$ Center=�6R   5^   +   5`   #     5b   "   $
 1.8* 65   $0 *គ្រូទទួលបន្ទុក��6B   $	 180Q�6Z   $ 15Q�6i    5d   &   �� 5f   *   "
 ��60   .�5g   .   ��  � ��
 �   �6[   �� 5i   .    � ��$
 Thin"�6K   6Z   �� 5j   .    � ��$
 Left=�6T   �� 5k   .    � ��$ Center=�6R   5l   +   5n   #     5p   "   $ 1* 65   $	 100Q�6@   $ ភារកិច្ច��6O   $ 15Q�6a    5q   &   �� 5r   *   "
 ��60   .�5s   .   ��  � �� 6K   �� *color+6e   �� 5t   .    � ��$
 Thin"�6K   6Z   �� 5u   .    � ��$ Center=�6T   �� 5v   .    � ��$ Center=�6R   5w   +   5z   #     5{   "   $
 0.5* 66   $ 	ភេទ��6C   $ 50Q�6P   $ 15Q�6^    5|   &   �� 5}   *   "
 ��60   .�5~   .   ��  � �� 6K   �� *color+6e   �� 5   .    � ��$
 Thin"�6K   6Z   �� 5�   .    � ��$ Center=�6T   �� 5�   .    � ��$ Center=�6R   5�   +   5�   #     5�   "   $ 1* 65   $6 0ថ្ងែខែឆ្នាំកំណើត��6@   $	 100Q�6Z   $ 15Q�6i    5�   &   �� 5�   *   "
 ��60   .�5�   .   ��  � ��
 �   �6[   �� 5�   .    � ��$
 Thin"�6K   6Z   �� 5�   .    � ��$ Center=�6T   �� 5�   .    � ��$ Center=�6R   5�   +   5�   #     5�   "   $
 1.5* 65   $' !លេខទូរស័ព្ទ��6B   $	 150Q�6W   $ 13Q�6f    5�   &   �� 5�   *   "
 ��60   .�5�   .   ��  � ��
 �   �6[   �� 5�   .    � ��$
 Thin"�6K   6Z   �� 5�   .    � ��$ Center=�6T   �� 5�   .    � ��$ Center=�6R   5�   +   5�   #     5�   "   $
 1.2* 65   $$ ទទួលបន្ទុក��6B   $	 143Q�6V   $ 13Q�6e    5�   &   �� 5�   *   "
 ��60   .�5�   .   ��  � ��
 �   �6[   �� 5�   .    � ��$
 Thin"�6K   6Z   �� 5�   .    � ��$ Center=�6T   �� 5�   .    � ��$ Center=�6R   5�   +   5�   #   5�      5�      5�      5�      � 5�      -   5        $��Footer��5�      .+�5�      � $��0 5 0 0q�6    5�      �� 5�      $��* �6+   67   �� 5�      $��* �6+   67   .+�5�      � $��0��6   .+�5�      �� $��0 30 0 0q�6%   .+�5�      �� $1��+បានឃើញ និងឯកភាព��6$   $��15(�6>   $��Center=�6L   $�� 0 5 0 0q�6i   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6{   6�   �� 5�      $��នាយកសាលា��6$   $��14(�67   $��Center=�6E   $�� 0 5 0 0q�6b   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6t   6�   5�      5�      � 5�      $��1��6   .+�5�      �� $ Vertical=�6%   .+�5�      �� $��Center=�6$   .��5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $��ថ្ងៃ��60   $��15(�6<   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6K   6�   �� 5�   &   -   5        $��lblDay��5�   0    �� Padding$ 0 5 0 0q�6>   $��...................��6P   $��12(�6k   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6z   6�   �� 5�   &   $��ខែ��60   $��15(�6:   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6I   6�   �� 5�   &   -	   5        $��	lblMonths��5�   0   $ 0 5 0 0q�6A   $��...................��6S   $��12(�6n   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6}   6�   �� 5�   &   $��ឆ្នាំ��60   $��15(�6=   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6L   6�   �� 5�   &   -
   5        $��lblYear��5�   0   $ 0 5 0 0q�6?   $��...................��6Q   $��12(�6l   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6{   6�   �� 5�   &   $��ព.ស��60   $��15(�6;   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6J   6�   �� 5�   &   -   5        $��lblKhmerYear��5�   0   $ 0 5 0 0q�6D   $��...................��6V   $��12(�6q   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   6�   5�   #   5�      �� 5�      $
��Left=�6$   $��40 5 0 0q�6?   $��15(�6Q   .��5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $��ថ្ងៃទី��60   $��15(�6>   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6M   6�   �� 5�   &   $ 0 5 0 0q�6Y   $��10(�6k   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6z   ��60   �� *Text+ �� ElementName$ lblDay��6�   �� 5�   &   $��ខែ��60   $��15(�6:   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6I   6�   �� 5�   &   $ 0 5 0 0q�6\   $��10(�6n   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6}   ��60   �� *Text+$ 	lblMonths��6�   �� 5�   &   $��ឆ្នាំ��60   $��15(�6=   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6K   6�   �� 5�   &   $ 0 5 0 0q�6\   $��10(�6n   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6}   ��60   �� *Text+$ 	lblMonths��6�   5�   #   5�      �� 5�      $6��0គ្រូបន្ទុកថ្នាក់��6$   $��Center=�6?   $��14(�6\   $�� 0 5 0 0q�6j   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6|   6�   5�      5�      5�      5�      �� 5�      $	��100��6   
������ 6:   5�      5�      5�      