using DG.Tweening;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform; //Player
    [SerializeField] private Transform _oriontationTransform; //Player's Oriontation
    [SerializeField] private Transform _playerVisiualTransform;//Player's Player Visiual

    [Header("Settings")]
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        Vector3 viewDirection = _playerTransform.position - new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
        
        _oriontationTransform.forward = viewDirection.normalized;

        //Karakterimizin hareketlerini tekrar çekiyoruz.
        float _horizontalInput = Input.GetAxisRaw("Horizontal");
        float _verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = _oriontationTransform.forward * _verticalInput + _oriontationTransform.right * _horizontalInput;
    
        if(inputDirection != Vector3.zero)//Eğer karakter durmuyorsa
        {
            _playerVisiualTransform.forward = Vector3.Slerp(_playerVisiualTransform.forward, inputDirection.normalized, Time.deltaTime * _rotationSpeed);
            //Karakteri görsel olarak döndereceğiz karakterin ana parçasını değil. O yüzden _playerVisiualTransform oluşturduk.
            //Slerp rotasyon işlemlerini daha yumuşak yapıyor. Animasyonlu gibi düşünebiliriz. (Lerp; pozisyonlar içn Slerp; rotasyonlar için)
            //Frame değerleri farklı olabilir. Fiziksel değer olmadığı için fixedUpdate kullanamayız. O yüzden Time.deltaTime * önceki frame ile sonraki frame değerini her bilgisayar için sabitlemiş oldu.
        }


    }
}
