   M S B A M L   `   `   `  ����I  ECamemisOffLine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CamemisOffLineCamemisOffLine  5          CamemisOffLine.App   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      .localclr-namespace:CamemisOffLine     5        �� 	Resources  5         ComboBoxToggleButton �� 
TargetType �Name � ColumnDefinitions ��Name J� SnapsToDevicePixels F�Name �� Triggers P� Property	 P� Value
 �� 
TargetName �� Property �� Value  ComboBoxTextBox ��Name  combobox �� 
TargetType �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name � OverridesDefaultStyle �� SelectionBrush �� 
CaretBrush �� TextWrapping �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency  � ActualWidth! �� MaxDropDownHeight" K� IsItemsHost# P� 
SourceName$ �� Triggers  Muol  Siemreap% �� MergedDictionaries& �� Source�� 5   
   % 5j      �� 5k      $e& _pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml?�6%   �� 5l      $h& bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6%   �� 5m      $z& tpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml?�6%   �� 5n      $s& mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml?�6%   %�  &	        &	 L    &	 �    7   7  &	 �    &	     �� 5	      " d�6;   .R�5
      � $ gd��6    5      �� 5      �� 5      $��25 �6+   .+�5      �� $ Border��6    . $��2��6H   
�������
�������$��1q�6�   �� 5      $ Boredr1��6    . $��1��6I   $��1q�6Z   ���#1183CA���#1183CA$��0,0,1,0q�6�   F� 5      $ Arrow��6    . $��1��6E   
�������$��Center=�6b   $��Center=�6   "����        р�[ ��[ � �     	6�   �� 5      $��0��6'   
��� ���6Y   5       5      P� 5       �@J�IsMouseOver	 . . �5      �� $
 Border��6!    � �� �#1183CA6L   �� 5      $
 Boredr1��6!    � �� �#1183CA6M   5      P� 5       �@d�	IsChecked$
	 TrueY�6@   . �5      �� $
 Arrow��6!    � ��" ��        �@KL @KL р��     	6D   �� 5      $
 Arrow��6!    � ��
 �����6D   �� 5      $
 Arrow��6!    � ��
 �����6F   �� 5      $
 Arrow��6!    � ��$	 1.5��6O   5      P� 5       � {�	 .  . �5       �� $
 gd��6!    � y�$ Visible=�6G   5!      5#      �� 5&      " ��66   .R�5'      �� $ PART_ContentHost��6   |�.  69   5(      �� 5+      " ��6%   .�5,      ��  � �� 5-      �� 5.      " ��6+   .R�5/      � .+�50   "   d� $��ToggleButton��6/   
���   �8��  |�.  $ Press=�6�    6�   �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+51   #   �� 52   "   $��ContentSite��63    .  #��z 6_   #��z 6�   #��z��6�   $��3,3,23,3q�6
  $��Center=�6  $
��Left=�68  �� 53   "   $ PART_EditableTextBox��6�    .  �#1183CA �#1183CA$��0,0,30,0q�6u   $ NoWrap=�6�   $��Segoe UI Dark)�6�   
���   �8�� $
��Left=�6*  |�. $��Center=�6W  $��15(�6s  $y�Hidden=�6�  # z 6�  ��6�   Z� 6�  � 54   "   $��Popup��6(   $��2��66   $��Bottom=�6J   #��z 6^    . |�.  $��Slide=�6�   .��55   &   � $��DropDown��6+    . #��z  6X   #��z  6�   #��z! 6�   .+�56   *   �� $ DropDownBorder��6V   $��2��61   $��2��6E    . 
�������#��z  6�   $��1q�6�   ���#1183CA6�   �� 57   *   $��2��67   ��.  $��1��6q    . .��58   .   �� " . 
�������5:   /   5;   +   5<   '   5=   #   5>       5?      P� 5@   "    �@��
IsEditable	 . . �5A   &   ��  � �� .  6B   �� 5B   &   $
 PART_EditableTextBox��6.    � ��
 �����6f   �� 5C   &   $
 PART_EditableTextBox��6-    � y�$ Visible=�6e   �� 5D   &   $
 PART_EditableTextBox��6-    � ��
 �   �6e   �� 5E   &   $
 ContentSite��6-    � y�$ Hidden=�6\   5F   #   P� 5G   "    �@��HasItems	 .  . �5H   &   �� $
 DropDownBorder��6-    � ��$ 95��6^   5I   #   P� 5J   "    � {�	 .  . �5K   &   ��  � �� .  6B   �� 5L   &   $
 PART_EditableTextBox��6-    � y�$ Visible=�6e   �� 5M   &   $
 PART_EditableTextBox��6-    � ��
 �   �6e   �� 5N   &   $
 PART_EditableTextBox��6.    � {� .  6e   �� 5O   &   $
 PART_EditableTextBox��6.    � ��
 �����6f   �� 5P   &   $
 ContentSite��6-    � y�$ Hidden=�6\   5Q   #   P� 5R   "    �@��
IsGrouping	 . . �5S   &   ��  � �� .  6V   5T   #   P� 5U   "   $# Popup��6*    �@�AllowsTransparency	 . . �5V   &   �� $
 DropDownBorder��6-    � ��$ 0,2,0,0q�6[   5W   #   5Y      5[      $ 5\      5^      �� 5`      .�5a      ��  � ��$% /FontStyle/#Khmer OS Muol Light)�6;   5c      5d      �� 5e      .�5f      ��  � ��$% /FontStyle/#Khmer OS Battambang)�6;   5h      5i      5p      5r      