   M S B A M L   `   `   `  ����G  CQuanLyQuanAn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:QuanLyQuanAn.View.DialogHostQuanLyQuanAn.View.DialogHost  5      -    'QuanLyQuanAn.View.DialogHost.AddAccount   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:QuanLyQuanAn.View.DialogHost     5      _ [MaterialDesignThemes.Wpf, Version=5.1.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      .��5
      �   � ColumnDefinitions  5   
   �� 5      $��* �6   �� 5      $��3* �6    � RowDefinitions 5   
   �� 5      �� 5      �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   �� $
��Left=�6   $��Center=�5      $��15(�5      $��10q�5      Tên đăng nhập6/   �� 5   
   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$  Nhập tên người dùng��6    %  !MaterialDesignFloatingHintTextBox#��[  5      $��1��5      $��0��5      $
��Left=�5      $��Center=�5      $��50��5      $	��250��5!      $��12(�5"      $��10 10 10 10q�5#      ��5       �� *HumanReadyToAdd.Name+ �� UpdateSourceTrigger$ PropertyChanged=�5#      �� 5$   
   $��1��6   $��0��5%      $
��Left=�5&      $��Center=�5'      $��15(�5(      $��10q�5)      Mật khẩu:6+   G� 5*   
   -   5         G�Name$ passwordbox��5-       ) %MaterialDesignFloatingHintPasswordBox#��[ 5+      $��1��5,      $��1��5.      $
��Left=�5/      $��Center=�50      $��50��51      $	��250��52      $��12(�53      $��10 10 10 10q�54       5*      �� *TypeAddPassword+54      �� 55   
   $��14(�6   $	��300��56      $��2��57      $��10q�58      $��2��5:      ��59      �� *	TypeAdd+��5;      �� *
AddHuman+��5<      ��  �� ElementName$ passwordbox��6H   �� 5=   
   $��14(�6   $	��300��5>      $��3��5?      $��10q�5@      
���  ��$��2��5B      ��5C      �� *
CloseAdd+.��6.   Thoát65   5D      5E      