   M S B A M L   `   `   `  ����G  CSCSCONTABIL2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:SCSCONTABIL2SCSCONTABIL2  5   	       SCSCONTABIL2.frmPrincipal   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:SCSCONTABIL2     5   	     � Title$  	Principal��5   	   $	��500��6   $	��750��6(    � 
ResizeMode$ NoResize=�64    � WindowStartupLocation$ CenterScreen=�6J   .��5	      � $	��744��6   $	��472��6    � RowDefinitions 5
   
   �� 5      $
��235* �6   �� 5      $
��237* �6    � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      .+�5   
   �� -   5         ��Name$ label4��5      $��Cadastro��6    $��34��63   $��152,62,152,0q�6?   $	��Top=�6U   $��14.667(�6m   $��2��6   �� 5   
   -   5        $ label5��5      $��Consulta��6    $��2��63   $��34��6C   $��
152,62,0,0q�6O   $	��Top=�6c   $��14.667(�6{   $��2��6�   $
��Left=�6�   $��68��6�   �� 5   
   -   5         ��Name$ 	btnCadUsu��5      $��2��6$   $��CADASTRAR USUÁRIO��68   $��60,140,59,54q�6T   6j   �� 5   
   -   5        $ 	btnCadFor��5      $��2��6$   $��CADASTRAR FORNECEDOR��68   $��40��6W   $��60,210,59,0q�6c   $	��Top=�6x   $��2��6�   �� 5   
   -   5        $ 	btnCadPro��5      $��2��6$   $��CADASTRAR PRODUTO��68   $
��Left=�6T   $��40��6o   $��	60,45,0,0q�6{   $	��Top=�6�   $	��253��6�   $��1��6�   6�   �� 5   
   -   5        $ 	btnConUsu��5      $��2��6$   $��CONSULTAR USUÁRIO��68   $��59,140,60,54q�6T   $��2��6j   6z   �� 5   
   -   5        $ 	btnConFor��5      $��2��6$   $��CONSULTAR FORNECEDOR��68   $��40��6W   $��59,210,60,0q�6c   $	��Top=�6x   $��2��6�   $��2��6�   6�   �� 5   
   -   5        $ 	btnConPro��5      $��2��6$   $��CONSULTAR PRODUTO��68   $��40��6T   $��
59,45,60,0q�6`   $	��Top=�6t   $��1��6�   $��2��6�   6�   �� 5   
   -	   5        $ btnSair��5      $
��SAIR��6"   $��2��61   $��51��6A   $��10,129.8,9.6,0q�6M   $��1��6e   $	��Top=�6r   6�   �� 5   
   -
   5        $ 	btnLogout��5      $��LOGOUT��6$   $��1��65   $��51��6E   $��10.4,129.8,10,0q�6Q   $��1��6j   $	��Top=�6w   6�   5      5       