   M S B A M L   `   `   `  ����G  CQuanLyQuanAn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:QuanLyQuanAn.ViewQuanLyQuanAn.View  5      ?$clr-namespace:QuanLyQuanAn.ViewModelQuanLyQuanAn.ViewModel  5      5clr-namespace:QuanLyQuanAn.ViewQuanLyQuanAn.View  5          QuanLyQuanAn.View.Menu   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:QuanLyQuanAn.View     5      3vm$clr-namespace:QuanLyQuanAn.ViewModel     5      -vclr-namespace:QuanLyQuanAn.View     5        � 	Resources  5      2  ,QuanLyQuanAn.ViewModel.Converter_Radiobutton   Converter_RadioButton��%   &	          5   
   6#   5       � DataContext 5      $  QuanLyQuanAn.ViewModel.cMenuVM  5   
    � Triggers 5      :� 5   
    :� RoutedEvent$ UserControl.Loaded��6   .[�5      �� .��5      �� .��5      W�  �� TargetProperty$ Opacity�6&    W� From$	 0.0X�5   "   	 W� To$	 1.0X�5   "    g� Duration$ 0:0:0.5D�5   "   5      5      �� 5      .��5      �� .��5      W�  �� 
TargetName$# UserControlTranslateTransform��6&   $ Y�5   "   $ 30X�5   "   $ 0X�5    "   $ 0:0:0.5D�5!   "   5#      5$      5%      5&      z�5(      T� 5)   
   -   5        !UserControlTranslateTransform��	 T� X$	 0Q�5*      
 T� Y$
 30Q�5+      .��5-      �  � RowDefinitions 5.   
   �� 5/      $
��auto �6   �� 50      $
��auto �6   �� 51      $��* �6   .+�53   
   �� $��1��6   $��0��54      ���#DAE2EA$��0,40,0,0q�56      $��2��57      58      �� 59   
   $��0��6    �� Orientation$ 
Horizontal=�5:      .+�5;      	�   MenuButtonStyle#��[ 6   $	��100��5<      $��40��5=       d� 	IsChecked 5>      �� *SelectedOption+ �� 	Converter# [   �� ConverterParameter$ Category��.��6�   
Danh mục6�   	� 5?      #��[ 6   $	��100��5@      $��40��5A       5B      �� *SelectedOption+# [  $
 Food��.��6�   
Món ăn6�   5C      �� 5D   
   $��2��6   ��6&   �� *Option+5E      5F      5G      