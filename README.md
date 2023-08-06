[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/EBv50WFu)

Postman Documentation : https://documenter.getpostman.com/view/19813296/2s93z3fQu2#6b51910d-0890-488a-8c65-32a70112d88b

Projenin çalıştırılması için yapılması gerekenler.

appsettings.json dosyasının içerisindeki ConnectionStrings kısmında MsSqlConnection kısmı kendi sql connection string cümleciğinizle güncellenmeli.

Redis host:localhost port:6379 , RabbitMQ host:localhost port:5672 olacak şekilde ayağa kaldırılmalı.(ben ikisinide controllerdan tetiklenmeye dayalı çalışacak şekilde ayarladım. Eğer kullanmak istemezseniz tetiklemediğiniz sürece sorun olmayacaktır.)

Bu değişiklikleri yaptıktan sonra projeyi çalıştırmanız veritabanının oluşması için yeterlidir. Otomatik olarak veritabanını database'de oluşturur. Sizin için default olarak Email = "admin@admin.com" , Password:"admin" olan seed data yüklemesi yapar.

Bunun haricinde projeyi özetlemek gerekirse;

TokenController Register metodu aracılığıyla normal kullanıcılar kayıt olabilir. Login metodu yardımıyla bütün kullanıcılar giriş yapabilir.

AdminController aracılığıyla adminler yeni admin kayıtı yapabilir. Kullanıcı silip güncelleyip bütün kullanıcı bilgilerini çekebilir.

CategoryController aracılığıyla sadece adminler kategori ekleyip güncelleyip silebilir. Bunun haricinde bütün kategorileri çekme ve id parametresine göre kategori getirme metodları yetkiden bağımsız herkes tarafından görülebilir. Kategorilerle beraber altındaki ürünlerde getirilir.

ProductController aracılığıyla sadece adminler ürün ekleyip güncelleyip silebilir. Stok güncellemesini yine adminler buradan yapabilir. Bunun haricinde ürün getirme metodları herkes tarafından görülebilir. Ek olarak ürünlerle beraber ait oldukları kategoriler dönülür.Ürün güncellerken ait olduğu kategori listeside buradaki update metodu aracılığıyla güncellenebiliyor.

CouponController aracılığıyla adminler kupon oluşturma , güncelleme , silme işlemlerini gerçekleştirebilir."percentAmount" alanını kuponun sepet toplamına % kaç indirim uygulayacağı şeklinde değiştirdim. Genellikle internette dolaştığımda BAHAR20 FIRSAT30 gibi yüzdelik indirimlerle kupon oluşturulduğunu görünce böyle daha mantıklı olacağını düşündüm. Yani oraya 20 yazarsanız sepet tutarına %20 indirim yapacaktır.

UserController aracılığıyla her kullanıcı kendisine ait puan bakiyesini sorgulayabilir. Ayrıca UserUpdate metodu aracılığıyla her kullanıcı kendisine ait bazı bilgileri güncelleyebilir.

OrderController aracılığıyla kullanıcı almak istediği ürün id'leri , kullanacaksa eğer kupon kodu ve ödeme yapacağı kart bilgilerini göndererek sipariş oluşturup ödeme yapabilir. Sipariş başarıyla oluşturulduktan sonra GetAllMyOrder sekmesinden her kullanıcı kendi siparişlerini listeleyebilir. Bu metoda id değeri vermeye gerek yok jwt token üzerindeki claim'den id değerini alıp o id'nin siparişlerini getirir.

OrderDetailController aracılığıyla sipariş numarası verilip siparişteki ürünlere ulaşılabilir.

NotificationController bölümü aracılığıyla ProcuderMail metodunu kullanarak rabbitmq kuyruğuna mail eklenebilir. Sonrasında ConsumeMail metodu yardımıyla listedeki son mail okunup gönderilir.(Şuanda bağlı gerçekten mail atar.)

CacheController bölümündeki post metoduna istek atılırsa redise user ıd ve cüzdan bakiyesi bilgileri cachelenir. Get metodu yardımıyla bu bilgiler redisten çekilir. Flush metodu yardımıyla veriler redisten temizlenebilir.

Burada rabbitmq ve redis kullanımı mantıksız olmuş olabilir. Senaryodan senaryoya göre değişebilir.




