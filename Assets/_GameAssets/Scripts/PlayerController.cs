using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")] //unity üzerinde değişkenleri başlık altına alıyor.
    [SerializeField] private Transform _orientationTransform;
    //Transform, bir objenin konumunu (position), rotasyonunu (rotation) ve ölçeğini (scale) tutar.
    //Örneğin Inspector'da bir objeye veya kameraya bağlı bir Empty GameObject'i bu alana atarsın. Atadaığımız orientation nesnesi gibi.

    //position, Dünya üzerindeki konumu(x, y, z), Tip: Vector3
    //rotation,  Dönüşü(Euler açıları veya Quaternion)  Tip: Quaternion
    //localScale, Ölçeği (objenin büyüklüğü), Tip: Vector3
    //forward, İleri yön vektörü, Tip: Vector3
    //right, Sağ yön vektörü, Tip: Vector3
    //up, Yukarı yön vektörü, Tip: Vector3

    //normal transform. ile farkı
    //transform ➔ Bu script'in bağlı olduğu GameObject'in kendi Transform'udur.
    //_orientationTransform ➔ Senin Inspector’dan elle atadığın başka bir Transform referansıdır. (mesela bir kamera, bir boş obje vs.)
    //Yani:
    //Eğer doğrudan transform.forward dersen karakterin kendi dönüşüne göre ileri yön alırsın.
    //Eğer _orientationTransform.forward dersen, başka bir nesnenin (mesela kameranın) bakış yönüne göre hareket yönü alırsın.

    [Header("Movement Settings")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private bool _canJump;

    [Header("Sliding Settings")]//Kayma
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultipiler;//Kayma hıznın çarpılma miktarı çünkü normal hızdan daha hızlı olacak.
    [SerializeField] private float _slideDrag;

    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;


    private Rigidbody _playerRigidbody;

    private float _horizontalInput, _verticalInput;

    private Vector3 _movementDirection;

    private bool _isSliding;//Direkt böyle bırakırsak false olur.



    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
        //freezeRotation => Rigidbody'nin kendi kendine dönmesini (rotasyon yapmasını) engellemektir.
        //Çünkü Rigidbody doğası gereği: Kuvvet uygularsan hareket eder(tamam), Kuvvet dengesiz gelirse döner / yuvarlanır.
        _movementSpeed = 30;
        _canJump = true;
    }

    private void Update()
    {
        SetInputs();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }
        

    private void SetInputs()
    {
        //"Şu anda oyuncu hangi yöne basıyor?" sorusunun cevabını sayısal (ve hızlı) bir şekilde alıp,bunu fiziksel kuvvet hesaplamasına hazırlıyor.
        _horizontalInput = Input.GetAxisRaw("Horizontal");//Horizontal Sağ/Sol hareket girişini temsil eder. (A/D tuşları veya ←/→ ok tuşları)
        _verticalInput = Input.GetAxisRaw("Vertical");// Vertical İleri/Geri hareket girişini temsil eder. (W/S tuşları veya ↑/↓ ok tuşları)
        //Bir araba sürüş oyununda GetAxis kullanırsın → direksiyon yavaş yavaş döner.
        //FPS karakterinde GetAxisRaw kullanırsın → sağa bastın mı hemen sağa yürür.
        //GetAxis=>Yumuşak (smooth) haraketler ara değerler kullanır.(Interpolasyon var)--GetAxisRaw Anında (raw) hareketler keskin değerler kullanır.(Interpolasyon yok)

        if (Input.GetKey(_slideKey))
        {
            _isSliding = true;
        }
        else if (Input.GetKey(_movementKey))
        {
            _isSliding = false;
        }
        else if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            //Zıplama işlemi
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown); //string Method Name alır, float türünde değer(time) alır.
            //Invoke => "Belirli bir süre bekle ➔ Sonra bu fonksiyonu çağır" demek.
        }
    }

    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;//Tüm yönleri anlayabilmek ve çapraz hareketi desteklemek için topluyoruz.
        //forward objeye göre hep ileri yönü gösterir.
        //Ancak çarpılan input (pozitif mi negatif mi) ona göre hareketi tersine çevirir.
        //W tuşu (ileri tuşu) ➔ _verticalInput = 1 ileri(forward yönüne) doğru kuvvet uygulanır.
        //S tuşu (geri tuşu) ➔ _verticalInput = -1 ileri vektörünün tersi yönünde kuvvet uygulanır(yani geriye doğru!).
        //aynı kurallar right için de geçerli.

        if (_isSliding)
        {
            _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * _slideMultipiler, ForceMode.Force);
            //Burada ekstra _slideMultipiler ile çarpıyoruz çünkü eğer slide(kayma) işlemi varsa ekstra hızlansın.
        }
        else
        {
            _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);
            //Diyelim ki ileri-sağa çapraz gidiyorsun.
            //Vektörün uzunluğu(magnitude) √2 olurdu → bu da hızın fazla olmasına yol açardı.
            //Normalize ederek bu uzunluğu 1 birim yapıyorsun ➔ böylece çapraz giderken hızın artmıyor.

            //*_movementSpeed:
            //Normalized vektörü istediğin büyüklükte ölçekliyorsun.
            //Bu, hareket hızının sabit kalmasını sağlar.

            //ForceMode.Force:
            //Sürekli ve kademeli bir kuvvet uygular(F = m * a). (Kütleye göredir.)
            //Daha doğal bir ivmelenme sağlar.
        }
    }

    private void SetPlayerDrag()
    {
        //drag nedir?(Sürtünme Kuvveti) Objenin hareket hızına karşı koyan bir yavaşlatıcı kuvvettir.
        if (_isSliding)
        {
            _playerRigidbody.linearDamping = _slideDrag;
            //linearDamping drag'e karşılık geliyor.
        }
        else
        {
            _playerRigidbody.linearDamping = _groundDrag;
        }

    }

    private void LimitPlayerSpeed()
    {
        //Limit fonksiyonu yazdığımız için bir hileci movementSpeed'i değişse bile limitlediğimizi için hileyi bir nevi önlemiş oluyorz.
        Vector3 flatVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
         if (flatVelocity.magnitude > _movementSpeed)
         {
            //flatVelocity.magnitude ➔ flatVelocity vektörünün büyüklüğünü (uzunluğunu) ölçüyor.
            //magnitude => √x2+z2 örn;√3^2+4^2 = √25 = 5
            //magnitude, o anki toplam yatay hızın(ağa-sola ve ileri-geri hızlar) kaç birim/saniye olduğunu verir yani net hızı verir.

            Vector3 limitedVeloctiy = flatVelocity.normalized * _movementSpeed;
            _playerRigidbody.linearVelocity = new Vector3(limitedVeloctiy.x, _playerRigidbody.linearVelocity.y, limitedVeloctiy.z);
            //flatVelocity.normalized ➔ sadece yönünü koruyoruz (büyüklüğü 1 birim oluyor).
            //Sonra bu yönü _movementSpeed ile çarpıyoruz ➔ böylece istenen maksimum hızda bir vektör elde ediyoruz.


        }
        //flatVelocity.magnitude, oyuncunun sahnedeki yatay toplam hızını verir; bu hız movementSpeed'i aşarsa,
        //hızını kırpıp limitleriz ve böylece hem daha düzgün bir fizik davranışı hem de hile koruması sağlamış oluruz.
    }

    private void SetPlayerJumping()
    {
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        //velocity(linearVelocity) => Rigidbody'nin o anki hızını verir. Bu bir Vector3 türünde değerdir.
        //velocity = objenin fiziksel mevcut hızıdır.
        //movementSpeed = objeyi hızlandırmak için uyguladığın kuvvetin şiddetidir.
        
        //Yukarıdaki kod, Rigidbody'nin mevcut yatay hızlarını (x ve z eksenlerini) koruyor, Ama dikey hızı(y ekseni) sıfırlıyor. (x ve z yatay hız - y ise dikey yani zıplama için vs.)
        //Eğer oyuncu zıplarken yere inerken biraz aşağı doğru hız birikmişse (negatif velocity) bu aşağı doğru hız sıfırlanır.
        //Böylece “zıplama” esnasında aşağı doğru bir hızdan kurtulmuş oluruz.
        //Eğer bunu yapmazsak ne olurdu? Oyuncu yere çakılırken zıplarsa, Aşağı doğru hız + yukarı doğru kuvvet olurdu,

        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        //transform.up ➔ Objeye göre "yukarı" yön vektörünü alır. (Yani(0, 1, 0) gibi bir vektör.)
        //* _jumpForce ➔ Bu yön vektörünü istediğimiz büyüklükte kuvvetle çarparız.
        //AddForce(..., ForceMode.Impulse) ➔ Kuvveti ani bir patlama gibi uygular(anlık, doğrudan hız ekler).
    }


    private void ResetJumping()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        //Oyuncunun tam ortasından aşağıya bir ışın atıp, belirli bir mesafede yere değip değmediğini kontrol eder ve
        //bu sayede oyuncunun yerde olup olmadığını tespit eder.
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
        //Raycast = Görünmez bir lazer ışını gibi bir çizgi atmak. "Bu çizginin bir objeye çarpıp çarpmadığını" kontrol etmek.

        //transform.position Işının atılacağı başlangıç noktası. (Oyuncunun tam konumu). Tip: Vector3 | origin
        //Vector3.down(0,-1,0) Işının yönü. (Aşağıya doğru). Tip: Vector3 | direction
        //_playerHeight * 0.5f + 0.2f Işının uzunluğu(mesafesi). (Oyuncunun yarı yüksekliği kadar + küçük bir ek boşluk) Tip: Float | maxDistance
        //_groundLayer Sadece "Ground" katmanına sahip objelere çarpılsın. Tip: int | layerMask (layer mask'ı menüden seçtiğimiz(atadığımız) için key value şeklinde örn: 6. olan ground layer'ı)
    }
}
