   M S B A M L   `   `   `  ����I  ECamemisOffLine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:CamemisOffLine.WindowsCamemisOffLine.Windows  5   	   3    -CamemisOffLine.Windows.ShowListStudentToPrint   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   0local$clr-namespace:CamemisOffLine.Windows  5   	   -   5          � Title$   ��5	   	   $	��650��6   $	��400��6   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6+    � WindowStartupLocation$ CenterScreen=�6�    � WindowStyle$ 
ToolWindow=�6�   .��5
      � .+�5   
   ��  �� Orientation$ Vertical=�6   .+�5      �� $����តារាឈ្មោះសិស្សសម្រាប់ធ្វើការបោះពុម្ភព្រឹត្តិប័ត្រពិន្ទុ��6   $��Center=�6W   $��10q�6t   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   $��16(�6�   6�   �� 5      $ Vertical=�6   $��10 0 0 0q�60   .+�5      �� $*��$ស្វែងរកសិស្ស��6   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�60   �� 5      $ 
Horizontal=�6   .+�5      �� -   5        $��txtStudentName��5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$6 0បញ្ចូលឈ្មោះសិស្ស��64   $��1q�6f   $��28��6z   $	��230��6�   $��Center=�6�   $
��Left=�6�   $��3 0 0 0q�6�   $��Center=�6  
�������$��10(�6�  $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6  6I  '  !MaterialDesignThemes.Wpf.PackIcon  5      -   5        $��	iconClose��5   .      Kind$ Close=�6?   $��Center=�6L   
�������$��	-18 0 0 0q�6y    � Cursor$
 Hand��6�   $y�	Collapsed=�6�   5      5      �� 5      -   5        $��checkAll��5      $3��-ជ្រើសរើសទាំងអស់��6'   $��	10 10 0 0q�6A   
����ح�$>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6k   6�   &   System.Windows.Controls.DataGrid  5      -   5        $��DGStudentName��5      "   VerticalScrollBarVisibility$
 Auto=�6,      AutoGenerateColumns .  	   GridLinesVisibility$
	 None=�6k   
   
IsReadOnly
 .    CanUserSortColumns .     SelectionMode$ Extended=�6�      SelectionUnit$ FullRow=�6�   ��. $��
10 -10 0 0q�6	  
������ $��0q�66  $	��370��6J  $
��Left=�6V  $	��400��6q   %� 
ScrollUnit$ Pixel=�6~  $��Hidden=�6�     AlternatingRowBackground
 �����   Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5         CellTemplate 5      �� 5      .R�5   "   �� $ 
Horizontal=�6-   $
��Left=�6F   $��Center=�6a   $	��320��6|   .+�5   &   �� $��checkStu��6/   -   5        
����ح���5   ?   �� *name+ d� 	IsChecked 6�   �� *print+5   #   5      5      5!      �� 5"      $ 
Horizontal=�6   $��Right=�62   $��10q�6N   .+�5#      �� -   5        $��btnNamePrint��5#      $��បោះពុម្ភ��6-   $	��100��6@   $��0 0 10 0q�6L   
�������
������ $��10(�6�   $��23��6�   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   6  �� 5$      -   5        $��	btnCancel��5$      $-��'ដកការជ្រើសរើស��6*   $	��100��6B   
���  ��
������ $��10(�6�   $��23��6�   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   6�   5%      5&      �� 5'   
   $��Right=�6   $��Bottom=�6-    �� CornerRadius$ 50��6H   $��15��6Z   $��15��6f   
���  ��
���   �$��1q�6�   $��
0 0 120 70q�6�   ._�5(      �� -	   5        $��txtNotification��5(      
�������$��0��6B   $��Center=�6K   $��Center=�6f   $
��Thin"�6�   $��10(�6�   $>��8/CamemisOffLine;component/FontStyle/#Khmer OS Battambang)�6�   5)      5*      5+      