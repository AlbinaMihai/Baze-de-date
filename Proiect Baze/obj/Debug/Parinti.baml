   M S B A M L   `   `   `  ����G  CProiect Baze, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Proiect_BazeProiect_Baze  5   	       Proiect_Baze.Parinti   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Proiect_Baze     5   	     � WindowState$  	Maximized=�5   	    � Icon$) #D:\PROIECT BAZE DE DATE\SiglaScoala��5	   
    � Title$ Note si absente`��5
   	   $	��600��6"   $
��1000��6/   $��14(�6<   ���#291b2c � 	Resources 5       
  Tables �� 
TargetType&   System.Windows.Controls.DataGrid �� Property  chs1 �� Value=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%�   &	        &	 ]     �� 5   
   "  6   .�5      ��  �@ ColumnHeaderStyle# �  62   6S   5      �� 5   
   "  6   .�5      ��  � �� �#4B00825      5      5      .��5      �� $��Stretch=�6   .��5   
   �� $��20 20 20 20q�6   .+�5      �� $��0 0 0 10q�6   ���#291b2c$��36(�6[   ���#CCA969��6'   �� *
WelcomeMes+6i   �� 5      -   5        $*��$Doriti sa modificati date personale?��5   D   $��	0 10 0 50q�6s   $��16(�6�   ���#CCA9696�   �� 5      .+�5      �� $��Right=�6   $��	0 0 200 0q�69   $��Date de contact:��6L   $��16(�6d   $��Italic$�6r   ���#CCA9696�   �� 5       $��1000 0 50 0q�6   ���#CCA9696J   �� 5!      $��Right=�6   $	��100��64   ���#291b2c���#291b2c���#CCA969$��18(�6�   $
��Bold"�6�   $��0 5 0 0q�6�   ��6�   �� *
DataProf+6�   5"      9� 5%      $��Mustrari disciplinare��6   ���#291b2c$��16(�6O   ���#CCA969.��5&        -   5        $��Mustrari��5'      $��Italic$�5(      $��SemiBold"�6&   $��UltraCondensed&�6<      AutoGenerateColumns .     
IsReadOnly . 	   RowBackground	 �#9932CC
   AlternatingRowBackground
 �#800080���#291b2c   GridLinesVisibility$	 All=�5+         ColumnHeaderHeight$ 60Q�6&   $��08q�6>   #���   5,      ���#4B0082
�������$	��Top=�5.      $��Center=�6$   $��
0,10,550,0q�5/         Columns 52      0  *System.Windows.Controls.DataGridTextColumn  53      ,  &System.Windows.Controls.DataGridColumn   Header$
 Nume��6N      Width5  /System.Windows.Controls.DataGridLengthConverter$ 97 6\   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *
NumeElev+6\     54      $ Prenume��6Q   $ 97 6b    61   �� *PrenumeElev+6b     55      $ Mustrare��6V   $	 270 6h    61   �� *ContinutMustrare+6h     56      $ Data Mustrare��6R   $	 150 6i    61   �� *DataMustrare+6i     57      $ 
Disciplina��6T   $	 100 6h    61   �� *NumeDisciplina+6h   5:      5;      9� 5>      $
��Note��6   ���#291b2c$��16(�6>   ���#CCA969.��5?        -   5          Name$ MarkGrid��5@      $��Italic$�5A      $��SemiBold"�6   $��UltraCondensed&�65    .   . 	 �#9932CC
 �#800080���#291b2c$	 All=�5D      $ 60Q�6&   $��30q�6>   #���   5E      ���#4B0082
�������$	��Top=�5G      $��Center=�6$   $��	0,0,620,0q�5H       5I        5J      $
 Nume��6J   $	 100 6X    61   �� *Nume+6X     5K      $ Prenume��6M   $	 100 6^    61   �� *	Prenume+6^     5L      $
 Nota��6J   $ 50 6X    61   �� *Nota+6X     5M      $ 	Data Nota��6O   $	 200 6b    61   �� *	Data_Nota+6b     5N      $ 
Disciplina��6P   $	 150 6d    61   �� *
Discipline+6d   5S      5T      9� 5W      $��Absente��6   ���#291b2c$��16(�6=   ���#CCA969.��5X      �� .+�5Y        -   5        $��PrezentaElevi��5Y      $��Italic$�5Z      $��SemiBold"�6   $��UltraCondensed&�65    .   . 	 �#9932CC
 �#800080���#291b2c$	 All=�5]      $ 60Q�6&   $��15q�6>   #���   5^      ���#4B0082
�������$	��Top=�5`      $��Center=�6$   $��	0,0,550,0q�5a       5b        5c      $
 Nume��6F   $	 100 6T    6-   �� *Nume+6T     5d      $ Prenume��6I   $	 100 6Z    6-   �� *	Prenume+6Z     5e      $ Data Absenta��6N   $	 200 6d    6-   �� *Data_Absenta+6d     5f      $ 
Disciplina��6L   $	 100 6`    6-   �� *
Disciplina+6`     5g      $ 
Nume Prof.��6K   $	 100 6_    6-   �� *	Nume_Prof+6_     5h      $ Prenume Prof.��6N   $	 100 6e    6-   �� *Prenume_Prof+6e   5k      5m      5n      5s      5t      5u      