   M S B A M L   `   `   `  ����I  ECamemisOffLine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:CamemisOffLine.WindowsCamemisOffLine.Windows  5   	   0    *CamemisOffLine.Windows.AllSubMonthlyResult   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:CamemisOffLine.Windows     5   	   -   5          � Title$  MonthlyResult��5      $
��25cm��6   $
��45cm��6,   $
��45cm��6S   .��5	      �� $��Hidden=�6   .��5
   
   �� -   5        $
��Grid��5
   ,    �� Orientation$ Vertical=�6   $��Center=�68   $
��45cm��6U   $��10q�6e   .+�5      � -   5        $��Header��5       � ColumnDefinitions 5      �� 5      $��* �6'   63   �� 5      $��* �6'   63   �� 5      $��* �6'   63   .+�5      �� $ Vertical=�6   $��0��64   $��5 40 0 0q�6D   .+�5      �� $/��)/CamemisOffLine;component/Image/MoEYS.png��6   $	��100��6O   6^   �� 5      $x��rមន្ទីរអប់រំ យុវជន និងកីឡា រាជធានីភ្នំពេញ��6   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6O   $��14(�6�   $��Center=�6�   6�   �� 5      -   5        $��lblSchoolName��5      $Z��Tវិទ្យាល័យ ហ៊ុន សែន បូរី១០០ខ្នង��61   $��Center=�6Z   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6w   $��14(�6�   6�   5      �� 5      $ Vertical=�6   $��1��64   .+�5      �� $E��?ព្រះរាជាណាចក្រកម្ពុជា��6   $��Center=�6<   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6Z   $��16(�6�   6�   �� 5      $M��Gជាតិ​ សាសនា ព្រះមហាក្សត្រ��6   $��Center=�6@   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6^   $��16(�6�   $��	0 -15 0 0q�6�   6�   �� 5      $ 
Horizontal=�6!   $��Center=�6:   $��0 50 0 0q�6W   .+�5      �� $*��$លទ្ធផលប្រចាំ��6    $��Center=�67   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6U   $��14(�6�   
����  �6�   �� 5      -   5        $��lblMonth��5       $��--��60   $��Center=�6=   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6[   $��14(�6�   
����  �6�   5      �� 5      $ 
Horizontal=�6!   $��Center=�6:   $��	0 -10 0 0q�6W   .+�5      �� $��ថ្នាក់ទី��6    $��Center=�63   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6Q   $��14(�6�   
����  �6�   �� 5      -   5        $��lblClass��5       $��--��60   $��Center=�6=   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6[   $��14(�6�   
����  �6�   5       5!      � 5"      $��2��6   5#      � 5$      .+�5%      &   System.Windows.Controls.DataGrid  -   5        $
��Data��5%         AutoGenerateColumns .     CanUserAddRows .     HeadersVisibility$ Column=�6Z   
���   �$��1 1 1 0q�6�   $��14(�6�   ��.  $��Hidden=�6�   $��Center=�6  $��Center=�6)  $
��35cm��6M  $
��44cm��6]  $��10q�6m   � 	Resources 5'      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� BasedOn �� 
TargetType	 �� Property
 �� Value �� Angle:  4System.Windows.Controls.Primitives.DataGridRowHeader��%�  '        7 '  z    '      7 �� 5(      8   "  6Y   .�5)      �� 	 � ��

 �����6;   �� 5*      	 � ��$>
 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6;   �� 5+      	 � ��$
 1q�6@   6L   �� 5,      	 � ��$
 
-1 -1 -1 0q�67   6L   �� 5-      	 � ��$
 5q�68   6D   �� 5.      	 � ��

 �   �6<   �� 5/      	 � ��$
 Stretch=�6K   �� 50      	 � ��$
 Center=�6I   6Z   �� 51      	 � ��$	
 200��66   �� 52      	 � ��$

 Bold"�6;   6J   �� 53      	 � ��

 �   �6K   �� 54      	 �@�LayoutTransform
 55   "   �� 56   &   $	 270Q�66   6D   58      59      �� 5:      "  6    .�5;      �� 	 � ��

 ���� 6;   �� 5<      	 � ��

 �   �6<   �� 5=      	 � ��$
 1 1 1 0q�6@   5>      �� 5@      8   "  6V   .�5A      �� 	 � ��

 ���� 6;   �� 5B      	 � ��$
 0��66   �� 5C      	 � ��$
 0��67   6C   5D      5E         Columns 5F      0  *System.Windows.Controls.DataGridTextColumn  5H      ,  &System.Windows.Controls.DataGridColumn   Header$ 	លេខ��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.7* 6:      MinWidth$ 70Q�6G      
FontFamily$> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6p   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6U   �� *number+   HeaderStyle 5I      �� 5K   "   "  6(   .�5L   &   �� 	 � ��

 �����6C   �� 5M   &   	 � ��$>
 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6C   �� 5N   &   	 � ��$
 1q�6H   6T   �� 5O   &   	 � ��$
 
-1 -1 -1 0q�6?   6T   �� 5P   &   	 � ��

 �   �6D   �� 5Q   &   	 � ��$
 Center=�6S   �� 5R   &   	 � ��$	
 200��6?   �� 5S   &   	 � ��$

 Bold"�6C   6R   �� 5T   &   	 � ��$
 Bottom=�6Q   6b   5V   #   5X        5Y      $ អត្តលេខ��6-   $
 0.6* 6>   $ 65Q�6K   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6Y   �� *student_schoolyear_id+ 5Z      �� 5\   "   "  6(   .�5]   &   �� 	 � ��

 �����6C   �� 5^   &   	 � ��$>
 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6C   �� 5_   &   	 � ��$
 1q�6H   6T   �� 5`   &   	 � ��$
 
-1 -1 -1 0q�6?   6T   �� 5a   &   	 � ��

 �   �6D   �� 5b   &   	 � ��$
 Center=�6S   �� 5c   &   	 � ��$	
 200��6?   �� 5d   &   	 � ��$

 Bold"�6C   6R   �� 5e   &   	 � ��$
 Bottom=�6Q   6b   5g   #   5i        5j      $( "គោត្តនាម នាម��6-   $ 2* 6D   $	 180Q�6O   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6w    6^   �� *name+ 5k      �� 5l   "   "  6(   .�5m   &   �� 	 � ��

 �����6C   �� 5n   &   	 � ��$>
 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6C   �� 5o   &   	 � ��$
 1 1 1 1q�6H   6Z   �� 5p   &   	 � ��$
 
-1 -1 -1 0q�6?   6T   �� 5q   &   	 � ��

 �   �6D   �� 5r   &   	 � ��$
 Center=�6S   �� 5s   &   	 � ��$	
 200��6?   �� 5t   &   	 � ��$

 Bold"�6C   6R   �� 5u   &   	 � ��$
 Bottom=�6Q   6b   5v   #   5x        5y      $ 	ភេទ��6-   $
 0.8* 6;   $ 60Q�6H   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6q    6V   �� *gender+ 5z      �� 5{   "   "  6(   .�5|   &   �� 	 � ��

 �����6C   �� 5}   &   	 � ��$>
 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6C   �� 5~   &   	 � ��$
 1 1 0 1q�6H   6Z   �� 5   &   	 � ��$
 
-1 -1 -1 0q�6?   6T   �� 5�   &   	 � ��

 �   �6D   �� 5�   &   	 � ��$
 Center=�6S   �� 5�   &   	 � ��$
 Bottom=�6Q   6b   �� 5�   &   	 � ��$	
 200��6?   �� 5�   &   	 � ��$

 Bold"�6C   6R   5�   #   5�        5�      $	 110Q�6.   $ * 6=   $6 0ថ្ងៃខែឆ្នាំកំណើត��6G   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6y    6a   �� *dob+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score1+   
Visibility 6c   �� *
visible1+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score2+ 6c   �� *
visible2+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score3+ 6c   �� *
visible3+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score4+ 6c   �� *
visible4+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score5+ 6c   �� *
visible5+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score6+ 6c   �� *
visible6+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score7+ 6c   �� *
visible7+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score8+ 6c   �� *
visible8+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *score9+ 6c   �� *
visible9+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score10+ 6d   �� *	visible10+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score11+ 6d   �� *	visible11+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score12+ 6d   �� *	visible12+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score13+ 6d   �� *	visible13+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score14+ 6d   �� *	visible14+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score15+ 6d   �� *	visible15+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score16+ 6d   �� *	visible16+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score17+ 6d   �� *	visible17+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score18+ 6d   �� *	visible18+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score19+ 6d   �� *	visible19+6�     5�      $ 60Q�6-   $
 0.6* 6;   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�    6H   �� *	score20+ 6d   �� *	visible20+6�     5�      $	 100Q�6-   $ * 6<   $$ ពិន្ទុសរុប��6F   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6z    6Z   �� *total_score+   ElementStyle 5�      �� 5�   "   " ��6(   .�5�   &   �� 	 � ��

 �   �6S   �� 5�   &   	 � ��$

 Bold"�6C   6R   �� 5�   &   	 � ��$
 Center=�6L   �� 5�   &   	 � ��$
 Center=�6J   5�   #   5�        5�      $ មធ្យមភាគ��6-   $	 100Q�6?   $ * 6N   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6v    6X   �� *	avg_score+ 5�      �� 5�   "   " ��6(   .�5�   &   �� 	 � ��
 6C   �� *color+6]   �� 5�   &   	 � ��$

 Bold"�6C   6R   �� 5�   &   	 � ��$
 Center=�6L   �� 5�   &   	 � ��$
 Center=�6J   5�   #   5�        5�      $ 80Q�6.   $ * 6<   $* $ចំណាត់ថ្នាក់��6F   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6u    6\   �� *rank+ 5�      �� 5�   "   " ��6(   .�5�   &   �� 	 � ��

 �  ��6Q   �� 5�   &   	 � ��$

 Bold"�6C   6R   �� 5�   &   	 � ��$
 Center=�6L   �� 5�   &   	 � ��$
 Center=�6J   5�   #   5�        5�      $	 100Q�6.   $ * 6=   $ និទេ្ទស��6G   $> 8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6t    6X   �� *	grading+ 5�      �� 5�   "   " ��6(   .�5�   &   �� 	 � ��

 �   �6S   �� 5�   &   	 � ��$

 Bold"�6C   6R   �� 5�   &   	 � ��$
 Center=�6L   �� 5�   &   	 � ��$
 Center=�6J   5�   #   5�      5�      5�      �� 5�      -   5        $��Footer��5�   0   $ Vertical=�6   $��0 10 0 0q�6>   .+�5�      � $��0 0 0 0q�6    5�      �� 5�      $��* �6+   67   �� 5�      $��* �6+   67   .+�5�      � $��0��6   $��Center=�6+   .+�5�      �� $��Center=�6    $��Center=�6=   .��5�      �� $ Vertical=�6)   .+�5�   "   �� $1��+បានឃើញ និងឯកភាព��6,   $��14(�6C   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6R   6�   �� 5�   "   $��នាយកសាលា��6,   $��14(�6<   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6J   $
��Bold"�6�   $��Center=�6�   6�   5�      5�      5�      � 5�      $��1��6   .+�5�      �� $ Vertical=�6%   .+�5�      �� $��Center=�6$   .��5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $��ថ្ងៃ��60   $��14(�6<   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6K   6�   �� 5�   &   -	   5        $��lblDay��5�   0   $��...................��6>   $��12(�6Y   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6h   6�   �� 5�   &   $��ខែ��60   $��14(�6:   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6I   6�   �� 5�   &   -
   5        $��	lblMonths��5�   0   $��...................��6A   $��12(�6\   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6k   6�   �� 5�   &   $��ឆ្នាំ��60   $��14(�6=   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6L   6�   �� 5�   &   -   5        $��lblYear��5�   0   $��...................��6?   $��12(�6Z   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6i   6�   �� 5�   &   $��ព.ស��60   $��14(�6;   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6J   6�   �� 5�   &   -   5        $��lblKhmerYear��5�   0   $��...................��6D   $��12(�6_   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6n   6�   5�   #   5�      �� 5�      $��Center=�6$   $��0 10 0 0q�6A   $��10(�6S   .��5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $C��=ធ្វើនៅរាជធានីភ្នំពេញ ��60   $��14(�6M   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6\   6�   �� 5�   &   $��ថ្ងៃទី��60   $��14(�6>   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6M   6�   �� 5�   &   $��14(�6Y   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6h   ��60   �� *Text+ �� ElementName$ lblDay��6�   �� 5�   &   $��ខែ��60   $��14(�6:   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6I   6�   �� 5�   &   $��14(�6\   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6k   ��60   �� *Text+$ 	lblMonths��6�   �� 5�   &   $��ឆ្នាំ��60   $��14(�6=   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6L   6�   �� 5�   &   $��14(�6\   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6k   ��60   �� *Text+$ 	lblMonths��6�   5�   #   5�      �� 5�      $6��0គ្រូប្រចាំថ្នាក់��6$   $��Center=�6?   $�� 0 5 0 0q�6\   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6n   $
��Bold"�6�   $��14(�6�   6�   �� 5�      -   5        $��lblTeacherName��5�   $   $
��----��6:   $��Center=�6I   $��
 50 40 0 0q�6f   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Muol Light)�6z   $
��Bold"�6�   $��14(�6�   6�   5�      5�      5�      5�      5�      5�      5�      