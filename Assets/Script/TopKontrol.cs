using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopKontrol : MonoBehaviour {
    public UnityEngine.UI.Button btn;//yeniden baslama butonu kullanımı sağlandı
    public UnityEngine.UI.Button next;//sonraki bolum butonu kullanımı sağlandı
    public UnityEngine.UI.Button main;//ana menu butonu kullanımı sağlandı
    public UnityEngine.UI.Text zaman, can,durum;//zaman,can,durum textlerinin kullanımı saglandı
    float zamanSayaci = 15;//her bolum icin sure sayacı belirlendi
    int canSayaci = 3;//her bolum icin can limiti belirlendi
    bool oyunDevam=true;//oyunun devam edip etmediğini kontrol için boolean oluşturuldu
    bool oyunTamam = false;//oyunun tamamlanma durumunu kontrol için boolean oluşturuldu,basta false olarak verildi
    Rigidbody fizik;//topun fizik kanunlarına uygun hareket ettirilebilmesi için rigidbody olusturuldu
    AudioSource audio1;//sesler icin audiosource olusturuldu
    private bool sessiz;//sessize alma butonu icin kontrol olusturuldu
    bool durdu;//durdurma menusu icin kontrol olusturuldu
    public GameObject durdurmaMenusu;//durdurma menusu kodda tanımlandı

    public float hiz;//hiz değişkeni public olarak oluşturulduğu için unity içerisinde istenen hız değeri verildi.bu değer aşağıda vector ile çarpıldı

   
    Joystick joystick;
	void Start () {

        

        joystick = FindObjectOfType<Joystick>();

        fizik = GetComponent<Rigidbody>();//rigidbody componentine erişim sağlandı
        audio1 = GetComponent<AudioSource>();//audiosource componentine erisim sağlandı


	}
	
	
	void Update () {

        if (oyunDevam && !oyunTamam) { 
        zamanSayaci -= Time.deltaTime;//zamanSayaci,süre her azaldığında o süreyi zamanSayaci'na ekleyecek
        zaman.text = (int)zamanSayaci+"";//float türündeki zamanSayaci,int'e çevrilip ekranda gösterildi.
        }
        else if(!oyunTamam)
        {
            durum.text = "Bölüm Tamamlanamadı";//oyun tamamlanamadığında ekranda yazı gorunur
            btn.gameObject.SetActive(true);//oyun tamamlanamadığında ekranda yeniden basla butonu gorunur
            main.gameObject.SetActive(true);//oyun tamamlanamadığında ekranda ana menu butonu gorunur

        }
        if (zamanSayaci < 0)//zaman sayaci 0'ın altına indiğinde oyunDevam false oldu yani oyun bitti
        {
            oyunDevam = false;

        }

        if (Input.GetKeyDown(KeyCode.Escape))//esc tusuna basma durumu icin sart olusturuldu
        {

            if (durdu)  
            {
                durdu = false; //oyunun durmadıgı durum
                Time.timeScale = 1f; //oyunun hızı 1 ayarlandı
                durdurmaMenusu.SetActive(false); //durdurma menusu acılmadı
                

            }
            else
            {

                durdu = true; //oyun durdu
                Time.timeScale = 0f;//oyunun hızı sıfırlandı
                durdurmaMenusu.SetActive(true);//durdurma menusu acıldı

            }




        }

    }

    void FixedUpdate()//fizik hesaplamaları öncesi çağrılan FixedUpdate metodu kullanıldı
    {
        if (oyunDevam && !oyunTamam)//topun hareket ettirilebilmesi oyunun tamamlanıp tamamlanmadığı şartına bağlandı
        {
            float yatay = Input.GetAxis("Horizontal");//topun,yatay(x) düzleminde hareket bilgileri alınması için get axis komutu verildi.Bu sayede klavyedeki yön tuşlarının kullanımı sağlandı
            float dikey = Input.GetAxis("Vertical");//topun,dikey(z) düzleminde hareket bilgileri alınması için get axis komutu verildi.Bu sayede klavyedeki yön tuşlarının kullanımı sağlandı


            Vector3 vec = new Vector3(-joystick.Vertical, 0, joystick.Horizontal);//vector oluşturularak hareket ettirilecek yönler için yukarıda oluşturulan değişkenler değer olarak verildi(x,y,z şeklinde)y yönünde hareket yok

            fizik.AddForce(vec * hiz);//topa kuvvet uygulanması yani bir hızı olması için rigidbody ile addForce komutu kullanıldı.vectorde bulunan değişkenler ile topun yatay ve dikey yönlerde hareketi sağlandı

        }
        else
        {
            fizik.velocity = Vector3.zero;//oyun bittiyse topun hızı sıfırlanır,vector sıfır olur
            fizik.angularVelocity = Vector3.zero;//oyun bittiğinde top kendi etrafında dönmeyi durdurur
        }
    }

    private void OnCollisionEnter(Collision cls)//onCollisionEnter ile temas olduğu zaman gerçekleşecek olaylar belirtilir
    {
        string objIsmi = cls.gameObject.name;
        if (objIsmi.Equals("Bitis"))//temas olmadan Bitis objesine ulaşılırsa
        {
            oyunTamam = true;
            durum.text = "Bölüm Tamamlandı,Tebrikler!!"; //ekranda yazı gorundu
            btn.gameObject.SetActive(true); //butonlar aktıf oldu
            next.gameObject.SetActive(true);//butonlar aktıf oldu
            main.gameObject.SetActive(true);//butonlar aktıf oldu
        }
        else if(!objIsmi.Equals("LabirentZemin")&& !objIsmi.Equals("Zemin"))//top,LabirentZemin ve Zemin'e değdiğinde canın azalmaması için şart konuldu
        {

            canSayaci -= 1;//temas olduğunda canSayaci 1 eksilir
            can.text = canSayaci + "";//can textinde eksilen can sayısı gözükür
            audio1.Play();

            if (canSayaci == 0)//canSayaci 0'a eşitlendiğinde oyun bitti
            {

                oyunDevam = false;
            }

        }
    }

    public void DisableAudio() //sesi kapatma icin metod olusturuldu
    {
        SetAudioMute(false); //ilk basıldığında ses kapandı
    }

    public void EnableAudio()
    {
        SetAudioMute(true); //ikinci basısta ses acıldı
    }

    public void ToggleAudio()
    {
        if (sessiz)
            DisableAudio(); //eger sessize alındıysa ses kapanır
        else
            EnableAudio();//diger turlu acılır
    }
    private void SetAudioMute(bool mute) 
    {
        AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        for (int index = 0; index < sources.Length; ++index)
        {
            sources[index].mute = mute;
        }
        sessiz = mute;
    }

    public void TogglePause() //durdurma butonu icin metod
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f; //oyunun hızı sıfırlandı
        durdurmaMenusu.SetActive(true);//durdurma menusu aktıf oldu

    }

    public void AnaMenu()
    {

        SceneManager.LoadScene("Start"); //durdurma menusundeki ana menu butonuyla ana menuye donus saglanır
        Time.timeScale = 1f;//oyunun hızı tekrar 1 yapılır,yeniden acısta donma olmaması ıcın

    }


    public void oyunaDevam()
    {
        durdu = false; //durma durumu false oldu
        Time.timeScale = 1f; //oyunun hızı 1 olur
        durdurmaMenusu.SetActive(false);//durdurma menusu kapanır


    }

    public void Yeniden()
    {

        SceneManager.LoadScene("LabirentOyunu"); //yenıden baslama butonu bolumu tekar yukler
        Time.timeScale = 1f;//oyunun hızı tekrar 1 yapılır,yeniden acısta donma olmaması ıcın

    }

    
}


