   M S B A M L   `   `   `  ����E  ATaxiClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TaxiClient
TaxiClient  5   	   5clr-namespace:TaxiClient.ModelsTaxiClient.Models  5   	   "    TaxiClient.AddVehicleWindow1   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TaxiClient     5   	   2modelsclr-namespace:TaxiClient.Models     5   	     � Title$  AddVehicleWindow1��5	   	   $	��400��6#   $	��600��60   .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      $��3 �6   6)   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   �� 5      6!   .+�5   
   �� $��0��6   $��0��6   .��6-   RegNr64   �� 5   
   -   5        $��txtRegNr��5      $��0��6"   $��1��6/   $��2 5 2 5q�6?   ��6P   �� *RegNr+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5   
   $��1��6   $��0��6   .��6-   Year63   �� 5    
   -   5        $��txtYearModel��5       $��1��6&   $��1��63   $��2 5 2 5q�6C   ��6T   �� *	YearModel+$ PropertyChanged=�6�   �� 5!   
   $��2��6   $��0��6   .��6-   
Trip meter69   �� 5"   
   -   5        $��txtTripMeter��5"      $��2��6&   $��1��63   $��2 5 2 5q�6C   ��6T   �� *	TripMeter+$ PropertyChanged=�6�   �� 5#   
   $��7��6   $��2��6   ��6-   �� *IsVehicleSaved+$ PropertyChanged=�6v   �� 5%   
   $��0��6   $��3��6   $��10 10 10 10q�6.   $��10(�6C   ��6Q   �� *	AddInfo+.��6n   
Add Info6x   �� 5'      $��3��6   $��0��6%   $��4��65   $��5 0 5 0q�6I   6\   �� 5)   
   $��4��6   $��0��6   .��6-   Color64   �� 5*   
   $��4��6   $��1��6   .��6-   Model64   �� 5+   
   $��4��6   $��2��6   .��6-   Fuel63   �� 5,   
   $��4��6   $��3��6   .��6-   Manufacturer6;   �� 5-   
   -   5        $��cbColorName��5-      $��4��6&   $��0��64   $��2 5 2 5q�6D    �� 
IsEditable .  �� 
IsReadOnly .  �� Text$ Color��6�    �� SelectedValuePath$ Id��6�    �� DisplayMemberPath$ 	ColorName��6�   ��6V   �� *Colors+	 �� SelectedItem	 6�   �� 
 �� Path$
 SelectedColor� �� Mode$ TwoWay=�6  �� 5.   
   -   5        $��cbModelType��5.      $��4��6&   $��1��64   $��2 5 2 5q�6D   $ Id��6�   $ 	ModelType��6�    .  . $ Model��6  ��6U   �� *Models+	 6t   �� $
 SelectedModel�$ TwoWay=�6  �� 5/   
   -   5        $��
cbFuelType��5/      $��4��6%   $��2��63   $��2 5 2 5q�6C   $ Id��6�   $ FuelType��6�    .  . $
 Fuel��6  ��6T   �� *Fuels+	 6r   �� $
 SelectedFuel�$ TwoWay=�6  �� 50   
   -   5        $��cbHybridFuelType��50   N   $��5��6f   $��2��6t   $��2 5 2 5q�6�   $ Id��6�   $ FuelType��6   .  . $ Hybrid Fuel��6N  {�6   ��  �� ElementName$ cbHybrid��$
 	IsChecked���6�   �� *HybridFuels+	 6�   �� $
 SelectedHybridFuel�$ TwoWay=�6c  �� 51   
   -   5        $��cbHybrid��51      $��2��6#   $��2��60   $��Hybrid��6@   $
��Left=�6p   $��Bottom=�6�    d� 	IsChecked 6Q   �� *
IsHybrid+53      �� 54   
   -	   5        $��cbManufacturer��54      $��4��6)   $��3��67   $��2 5 2 5q�6G   $ Id��6�   $ ManufactName��6�    .  . $ Manufacturer��6  ��6X   �� *Manufacturers+	 6~   �� $
 SelectedManufacturer�$ TwoWay=�62  �� 55   
   $��7��6   $��7��6   $��5 5 5 5q�6.   ��6?   �� *
AddVehicle+.��6^   Add Vehicle6k   56      57      