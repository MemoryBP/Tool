   M S B A M L   `   `   `  ����   DataBaseTool    DataBaseTool.MainWindow   2xctk)http://schemas.xceed.com/wpf/xaml/toolkit  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$#  客户通话记录工具_V1.2��$	��700��$
��1000�� � WindowStartupLocation$ CenterScreen=� � 	Resources  
  Simple �� 
TargetType �� Property �� Value��%V   &	        �� " _�.���  �@_�HorizontalOffset$ 50����  � ��
 ��ح�.���  � RowDefinitions �� $
��20px��$
��0.1* ��� $
��20px��$
��0.1* ��� �� $
��20px��$
��0.1* ��� $
��0.1* ��� $��0* ��� $
��0.4* ��� $
��0.1* � � ColumnDefinitions �� $��0.35* ��� $
��auto ��� �� $
��auto �.+�� $��0��$��0��$��4��$��Center=�.+��� $��5,5,0,5q�$
��15px(�数据源：�� -   $��	txtServer��$��100px��$��200px��$��5,5,0,5q�$
��15px(�$��	localhost���� $
��15px(�$��10,5,0,5q�数据库：�� -   $��txtDataBase��$��100px��$��200px��$��5,5,0,5q�$
��15px(�$
��dali���� $
��15px(�$��10,5,0,5q�用户名：�� -   $��txtUser��$��100px��$��200px��$��5,5,0,5q�$
��15px(�$
��root���� $
��15px(�$��10,5,0,5q�	密码：G� -   $��txtPwd��$��100px��$��200px��$��5,5,0,5q�$
��15px(� G� Password$
 root���� -   $��btnConn��$��100px��$��10,5,0,5q�$
��15px(�$��连接数据库���� -   $��
chbIsDebug��$��Center=�$��100px��$��10,5,0,5q�$
��15px(�$��Debug��	 � ToolTip$	 保存到日志文件��� $��1��$��0��$��4��$��Center=�$��Stretch=�.+��� $��5,5,0,5q�$
��15px(�$��身份证：���� -   $��txtUserName��$��160px��$��200px��$��5,5,0,5q�$
��15px(�$	 	身份证���� -	   $��	btnSelect��$
��auto��$��10,5,0,5q�$
��15px(�$��查询��
 �� 	IsDefault
 . �� -
   $��btnAll��$
��auto��$��10,5,0,5q�$
��15px(�$��显示全部���� -   $��	btnExport��$��100px��$��10,5,0,5q�$
��15px(�$��导出��&   System.Windows.Controls.DataGrid  -   $��dgCustomers��$��2��$��5��$��5q�$
��Auto=�$
��Auto=�$��Stretch=� �� AlternationCount$ 2��$��Stretch=�   AutoGenerateColumns .     CanUserAddRows .     SelectionMode$ Single=�   CanUserResizeColumns .    
IsReadOnly .    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 姓名��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *
UserName+  $ 	手机号��$ 1*  �� *	Phone_num+  -   $��dgMeg��$��2��$��1��$��3��$��Stretch=�$��Stretch=� .  $ 2��$��5q� .     $ 号码��$ 1*  �� *	phone_num+  $ 通话次数��$
 0.8*  �� *
call_cnt+  $ 号码标注��$
 0.8*  �� *contact_name+  $ 需求类别��$
 0.8*  �� *
needs_type+  $ 关系推测��$
 0.8*  �� *
p_relation+  $ 最近一周联系次数��$ 1*  �� *
contact_1w+  $ 最近一月联系次数��$ 1*  �� *
contact_1m+  $ 号码归属地��$
 0.8*  �� *phone_num_loc+  $ 通话时长��$
 0.8*  �� *
call_len+��  �� Orientation$ 
Horizontal=�$��3��$��1��$��3��$��Center=�.+��� $��每页显示��$��7q��� -   $��cmbPageSize��$��5q�$
��auto��.@���  �� 
IsSelected . $��15���� $��25���� $��50���� $	��100���� $��	条记录��$��7q��� $��转到��$��7q��� -   $��
tbxPageNum��$��7q�$
��40px�� �� TextAlignment$ Center=��� $	��页��$��7q��� -   $��	btnPageGo��$��GO��$��Center=�$��7q��� -   $��	btnPageUp��$��	上一页��$��Center=�$��7q��� -   $��btnPageNext��$��	下一页��$��Center=�$��7q��� $��20���� $��0,0,10,0q�$��【共�� �� -   $��tbkTotal��$��0,0,10,0q�$��0��
���  �� �� $��0,0,10,0q�$��页】�� �� $��0,0,10,0q�$��【当前第�� �� -   $��tbkCurrentsize��$��0,0,10,0q�$��0��
���  �� �� $��0,0,10,0q�$��页】��� $��4��$��1��$��3��$��Center=�.+��� $��5,0,0,0q�$
��15px(�已选择用户:�� -   $��txtSelectedUser��$��5,0,0,0q�$
��15px(�
���  ��NULL�� $��5,0,0,0q�$
��15px(�更新时间:�� -   $��txtUpdateTime��$��5,0,0,0q�$
��15px(�
���  ��NULL�� -   $��txtCount��$��5,0,0,0q�$
��15px(�共0条数据�� -   $��
txtPublish��$��Stretch=�$��Stretch=�$��1��$��6��$��3��$
��Auto=�$
��Auto=�$��5q� �� TextWrapping$ NoWrap=�$
��15px(� �� AcceptsReturn . 
���  �� �� 
IsReadOnly . .��	 _� #��[  .���  �� �� .+��� -   $��txtPublishTooltip��$��1�� . .��打印信息.���  � Orientation$ 
Horizontal=�$��9��$��1��$��2��$��Center=�.+��� $��10,5,0,5q�$��系统时间:���� -   $��
tblDateNow��$��10,5,0,5q�$��当前时间��$
��auto��� $ 
Horizontal=�$��9��$��0��$��2��$��Center=�.+��� -   $��tblCurrentCount��$��5,5,0,5q�$��当前0条数据��
���  ��