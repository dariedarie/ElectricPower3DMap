   M S B A M L   `   `   `  ����L  HPredmetniZadatak3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:PredmetniZadatak3PredmetniZadatak3  5   	   "    PredmetniZadatak3.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:PredmetniZadatak3     5   	     � Title$  
MainWindow��5   	   $	��800��6   $	��800��6)   .��5	      � -   5        $
��Grid��5	      $��1,0,-1,0q�6   
�������.+�5
   
   �� -   5        $��loadBtn��5
   �   $��UČITAJ��6   $
��Left=�6"   $��
629,59,0,0q�6=   $	��Top=�6g   $	��117��6   $��49��6�   $��24(�6�   $
��Bold"�6�   
�������6�   '� 5   
   -   5        $��vp��5   =   $��0��6   $	��Top=�6%   $	��555��6G   $��	0,156,0,0q�6T   }�.  '� Camera 5      =� 5      -   5        
Camera�� � Position$ 0,-0.1, 3.51�5   4    � LookDirection$ 0 ,0, -15�6K    � UpDirection$ 0, 1, 05�6d   x�5      g� 5       g� Content 5      �� 5      5      g� 5       5      h� 5      -   5        AllModelsGroup��.��5      � -   5        	Layer�� � Material 5   "   u� 5   &    u� Brush 5   *   �� 5   .    �� ImageSource$; 5C:/Users/colak/Desktop/RG33/PredmetniZadatak3/map.jpg��69   5!   '   	 � Geometry	 5#   "   k� 5$   &   
 k� 	Positions
 �    k� TriangleIndices �      k� TextureCoordinates �   5%   ?   5'      � 5)      -   5        MapBack��	 5*   "   k� 5+   &   
 �    �     6]    5-   "   u� 5.   &   
 �   �66   50       j� 	Transform 52      Y� 53   "   .�54   &   S� -   5        translacija�� S� OffsetX$ 0Q�54   P    S� OffsetY$ 0Q�6\    S� OffsetZ$ 0Q�6h   �� 55   &   -	   5        
skaliranje�� �� ScaleX$ 1Q�55   K    �� ScaleY$ 1Q�6V    �� ScaleZ$ 1Q�6a   �� 56   &   -
   5        	rotacijaX�� �� CenterX$ 0Q�56   K    �� CenterY$ 0Q�6W    �� CenterZ$ 0Q�6c    �� Rotation 57   *   �� 58   .   -   5        xAxisAngleRotation�� �� Angle$	 -60Q�58   _    �� Axis$ 1,0,05�6k   5:   '   �� 5;   &   -   5        	rotacijaY��$ 0Q�5;   K   $ 0Q�6W   $ 0Q�6c    5<   *   �� 5=   .   -   5        yAxisAngleRotation��$ 0Q�5=   ^   $ 0,1,05�6h   5?   '   5@   #   5B      5D      5F      5G      5H      