**-------------------------------------------------------------------------------------------**



**-------------------------------------------------------------------------------------------**





**---------------------------------------------------**

**2025 12 05**

**---------------------------------------------------**



**-------------------------------------------------------------------------------------------**



**-------------------------------------------------------------------------------------------**





**In http://localhost:5173/app/kindness  :**  

**Change 懷恩塔-塔位清單 --> 懷恩塔塔位管理**



**In http://localhost:5173/app/ancestral :**

**Change 陳氏宗祠-牌位清單 -->宗祠牌位管理**





**1.On Sidebar:**

	**Change 懷恩塔-塔位清單 --> 懷恩塔塔位管理**

	**Change 陳氏宗祠-牌位清單 -->宗祠牌位管理**



**2.會員管理 http://localhost:5173/app/user**

	**Request failed with status code 404**



**3.懷恩塔塔位管理**

	**http://localhost:5173/app/kindness**

	**Request failed with status code 404**



**4.宗祠牌位管理**

	**http://localhost:5173/app/ancestral**

	**Request failed with status code 404**



**5.訂單管理**

	**http://localhost:5173/app/order**

	**Request failed with status code 404**



**-------------------------------------------------------------------------------------------**





**http://localhost:5173/**



**Give respective link to the three item :**



**1.陳氏宗祠-牌位查詢**

**2.懷恩塔-塔位查詢**

**3.台中市銀同碧湖陳氏宗親會 影片 (FYI:wwwroot\\images\\Films\\ChenClanOpening.mpr)**

**-------------------------------------------------------------------------------------------**



**Wherever I am at any page ,Could I let my page stay where it is when pressing 'refresh' icon of Browser?**



**-------------------------------------------------------------------------------------------**



**http://localhost:5173/app/order**



**Change '訂單管理'  --> '活動辦名管理'**



**-------------------------------------------------------------------------------------------**



**會員管理 http://localhost:5173/app/user**

**新增會員 http://localhost:5173/app/user/add**

**Pressing '新增' => 'Request failed with status code 400'**





**http://localhost:5173/app/user**

**Pressing '編輯' ==> 'http://localhost:5173/'**

**-------------------------------------------------------------------------------------------**



**In 宗祠牌位管理 http://localhost:5173/app/ancestral**

**Pressing '新增牌位',**

**When in 'http://localhost:5173/app/ancestral/add'**

**Pressing '新增',redirecting to 'http://localhost:5173/'**





**-------------------------------------------------------------------------------------------**



**In 懷恩塔管理 http://localhost:5173/app/kindness**

**Pressing '新增塔位',**  

**When in 'http://localhost:5173/app/kindness/add'**

**Pressing '新增',redirecting to 'http://localhost:5173/'**



**-------------------------------------------------------------------------------------------**



**活動類別**



**http://localhost:5173/app/category/edit/1**



**Pressing '更新', => 'Request failed with status code 400'**



**-------------------------------------------------------------------------------------------**

**宗親會基本檔**



**http://localhost:5173/app/company**

**Pressing '編輯',redirecting to 'http://localhost:5173/'**



**-------------------------------------------------------------------------------------------**

**活動基本檔:**



**In 'http://localhost:5173/app/product/add'**

**Warning msg: 	主辦單位為必填**





**---------------------------------------------------**

**2025 12 04**

**---------------------------------------------------**

**In 'http://localhost:5173/login' page,**



**Add New Button '訪客登入'-->Leading to 'http://localhost:5173'**



**-----------------------------------------------------**

**1.Modify 'Guest Role' into 'Unauthenticated,ie :No-Login'**



**2.When 'Unauthenticated' (ie.http://localhost:5173/) see what the index1.cshtml  displays of MVC project.**



**3.When Longlined  as  Admin,Customer, Unautenticated. clicking '陳氏宗祠祖先牌位暨懷恩塔家族墓園塔位管理平台',leading to  'http://localhost:5173/'**



**4.When logined as Admin,clicking '首頁' leading to 'http://localhost:5173/app/admin'**   



**5.When logined as Customer,clicking '首頁' leading to 'http://localhost:5173/app/customer'**





------------------------------------------------------------------------------------------------

http://localhost:5173/



Please give its respective destination link to each clickable item.



------------------------------------------------------------



http://localhost:5173/



Modify:

祖先牌位管理

完整記錄祖先牌位資訊，建檔歷史紀錄，便於家族查詢



to



祖先牌位查詢

快速查詢陳氏宗祠祖先牌位詳情和位置





In http://localhost:5173/app/admin

clicking any Item,redirecting user to  http://localhost:5173/

should've navigated to respective Admin UI



------------------------------------------------------------



When login as Admin,  pressing 'refresh' button of Browser, 

it redirects me to http://localhost:5173/login?redirect=/app/admin





------------------------------------------------------------



When in http://localhost:5173/app/category



clicking 'Edit' Button: 

Redirecting to in http://localhost:5173/app/category/edit/1 , I see  'Request failed with status code 404'





------------------------------------------------------------



When in http://localhost:5173/app/product/add 



clicking 'Add', New data is not inserted and not displayed in List.





------------------------------------------------------------



When I click '立即登出'  on Warning Modal, 

it didn't redirect to  https://www.facebook.com/groups/654519621275974



------------------------------------------------------------



On Autologout Warning Modal, the  'left-seconds to logout' calculation and displaying is something wrong ,

please ensure its correct.



------------------------------------------------------------



in http://localhost:5173/app/admin,



in '活動基本檔' 新增、編輯和管理活動商品' section,

please modify '進入商品管理' -> '活動管理'

