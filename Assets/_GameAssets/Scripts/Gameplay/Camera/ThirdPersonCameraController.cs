using DG.Tweening;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;//Oyuncunun ana Transform'u. (Vücudu, pozisyonu) //Player
    [SerializeField] private Transform _oriontationTransform;//Oyuncunun bakış yönünü belirleyen boş bir nesne (Orientation) //Player's Oriontation
    [SerializeField] private Transform _playerVisiualTransform;//Sadece oyuncunun görsel modeli (karakterin 3D mesh'i gibi) //Player's Player Visiual

    [Header("Settings")]
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        Vector3 viewDirection = _playerTransform.position - new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
        //Kamera ile oyuncu arasındaki "yatay" bakış yönünü bulmak.
        //transform.position ➔ Kameranın pozisyonu.
        //playerTransform.position ➔ Oyuncunun pozisyonu.
        //Ama sadece y ekseninde eşitlik sağlıyoruz(_playerTransform.position.y) ➔ Yani yükseklik farkını yok sayıyoruz.
        //Böylece sadece düzlemde(X - Z) "oyuncu neredeyse ona doğru bak" mantığı oluşuyor.
        //Kamera oyuncunun konumuna göre bir bakış yönü üretir, ama yukarı-aşağı bakmaz.


        _oriontationTransform.forward = viewDirection.normalized;
        //Orientation nesnesinin forward yönünü (ileri yönünü) az önce bulduğumuz viewDirection'a ayarlıyoruz.
        //Kamera karakterin yönünü etkiler: Kamera nereye bakarsa, oyuncu da o yöne doğru hareket eder.


        //Karakterimizin hareketlerini tekrar çekiyoruz.
        float _horizontalInput = Input.GetAxisRaw("Horizontal");
        float _verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = _oriontationTransform.forward * _verticalInput + _oriontationTransform.right * _horizontalInput;
        //Oyuncunun bastığı tuşlara göre hangi yöne hareket etmek istediğini belirlemek.


        if (inputDirection != Vector3.zero)//Eğer karakter durmuyorsa
        {
            _playerVisiualTransform.forward = Vector3.Slerp(_playerVisiualTransform.forward, inputDirection.normalized, Time.deltaTime * _rotationSpeed);
            //Karakteri görsel olarak döndereceğiz karakterin ana parçasını değil. O yüzden _playerVisiualTransform oluşturduk.
            //Slerp rotasyon işlemlerini daha yumuşak yapıyor yani iki vektör arasında yumuşak bir dönüş yapıyor. Animasyonlu gibi düşünebiliriz. (Lerp; pozisyonlar içn Slerp; rotasyonlar için)
            //Frame değerleri farklı olabilir. Fiziksel değer olmadığı için fixedUpdate kullanamayız. O yüzden Time.deltaTime * önceki frame ile sonraki frame değerini her bilgisayar için sabitlemiş oldu.
            //deltaTime ➔ Frame bağımsızlık sağlar
        }


    }
}
