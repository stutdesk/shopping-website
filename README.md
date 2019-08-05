# shopping-website
簡易購物網頁，功能包含產品列表、註冊登入、搜尋，非會員會員以及管理者獨立介面等等 
<blockquote>這是我使用Asp.net MVC(C#)第一個作品，本作品是一個3C的線上購物網站，構造簡單功能齊全，有購物車、三種權限獨立介面、後台管理系統、等等</blockquote>
作品圖較多，還望海涵

## 作品介紹
**1. 首頁及產品搜尋部分:**<br>
(圖一)首頁為商品陳列，在未登入會員前只能瀏覽商品而無加入購物車選項<br>
![image](https://github.com/stutdesk/shopping-website/blob/master/images/homepage(withoutlogin).png)<br>
   (圖二)此為以產品種類查詢 ![image](https://github.com/stutdesk/shopping-website/blob/master/images/droplistkindsearch.png)<br>
   (圖三)產品關鍵字搜尋 ![image](https://github.com/stutdesk/shopping-website/blob/master/images/productsearch.png)<br>
   <br>
**2. 註冊及登入功能:**
(圖四)註冊頁面，以上四個選項都必須填寫才能註冊
![image](https://github.com/stutdesk/shopping-website/blob/master/images/regispage.png)<br>
(圖五)登入頁面，帳密輸入錯誤會清空並且出現錯誤訊息
![image](https://github.com/stutdesk/shopping-website/blob/master/images/loginpage.png)<br>
<br>
**3. 會員購物流程及訂單查詢:**
在此會介紹會員登入後頁面、購物流程及訂單查詢
(圖六)會員首頁，只有在會員登入後才會出現加入購物車選項，跟非會員頁面的navbarcolor也有所不同
![image](https://github.com/stutdesk/shopping-website/blob/master/images/homepage(login))<br>
(圖七)購物流程1:當選擇數量並按下加入購物車會，會將數量及商品傳至shoppingcar頁面
![image](https://github.com/stutdesk/shopping-website/blob/master/images/shoppingcar.png)<br>
(圖八)購物流程2:可以在此頁面刪除或選擇要結帳物品(若沒選就按下comfirm會出現警訊)
![image](https://github.com/stutdesk/shopping-website/blob/master/images/alarm.png)<br>
(圖九)購物流程3:此也面會顯示選擇的結帳商品個別及所有的總額，並在此填寫收件人訊息
![image](https://github.com/stutdesk/shopping-website/blob/master/images/shoppingcar2.png)<br>
(圖十)購物流程4:下單完成，會顯示訂單詳細資訊
![image](https://github.com/stutdesk/shopping-website/blob/master/images/ordersuscess.png)<br>
(圖十一)訂單查詢:此頁面可以查詢已下單的訂單資訊
![image](https://github.com/stutdesk/shopping-website/blob/master/images/orderlist.png)<br>
<br>
**4. 後臺管理介紹:**
介紹後臺管理功能，功能有產品及會員的新增修改及刪除
(圖十二)管理員介面(刪除產品會出現警訊)
![image](https://github.com/stutdesk/shopping-website/blob/master/images/homepage(admin).png)<br>
(圖十三)新產品新增
![image](https://github.com/stutdesk/shopping-website/blob/master/images/Creatproduct.png)<br>
(圖十四)產品資訊修改，圖片使用預覽功能會先預覽舊的圖片
![image](https://github.com/stutdesk/shopping-website/blob/master/images/Edit.png)<br>
(圖十五)會員管理
![image](https://github.com/stutdesk/shopping-website/blob/master/images/Member.png)<br>
