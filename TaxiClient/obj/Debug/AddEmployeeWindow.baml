   M S B A M L   `   `   `  ����E  ATaxiClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TaxiClient
TaxiClient  5   	   7 clr-namespace:TaxiClient.HelpersTaxiClient.Helpers  5   	   5clr-namespace:TaxiClient.ModelsTaxiClient.Models  5   	   "    TaxiClient.AddEmployeeWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TaxiClient     5   	   4helpers clr-namespace:TaxiClient.Helpers     5   	   2modelsclr-namespace:TaxiClient.Models     5   	     � Title$  AddEmployeeWindow��5
   	   $	��300��6#   $	��300��60   .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   �� 5       6!   .+�5"   
   �� $��1��6   $��0��6   $��8(�6-   .��6:   	Firstname6E   �� 5#   
   $��2��6   $��0��6   $��8(�6-   .��6:   
Lastname6D   �� 5$   
   $��3��6   $��0��6   $��8(�6-   .��6:   
Username6D   �� 5%   
   $��4��6   $��0��6   $��8(�6-   .��6:   
Password6D   �� 5&   
   $��5��6   $��0��6   $��8(�6-   .��6:   Confirm Password6L   �� 5'   
   -   5        $��txtFirstName��5'      $��1��6e   $��1��6r   $��2��6�   ��6&   �� *	FirstName+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5(   
   -   5        $��txtLastName��5(      $��2��6c   $��1��6p   $��2��6�   ��6%   �� *
LastName+$ PropertyChanged=�6�   �� 5)   
   -   5        $��txtUserName��5)      $��3��6q   $��1��6~   $��2��6�   ��6%   �� *
UserName+ �� Mode$ TwoWay=�$ PropertyChanged=�6�   G� 5+   
   -   5        $��pwbPassword��5+      $��4��6)   $��1��66   $��2��6F   6Z   G� 5,   
   -   5        $��pwbConfirmPword��5,      $��5��6-   $��1��6:   $��2��6J   6^   �� 5.   
   -   5        $��	lblStatus��5.   ^   $��0��6   $��0��6   $��4��6-   $��Center=�6A   6q   �� 50   
   -   5        $��ckbAdmin��50      $��1��6#   $��3��60   $��8(�6@    d� 	IsChecked 6M   �� *	IsAdmin+$ PropertyChanged=�.��6�   Admin6�   �� 52   
   -   5        $��btnAddEmployee��52      $��7��6'   $��2��64   .��6a   Add6f   55      56      