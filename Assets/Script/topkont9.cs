using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class topkont9 : MonoBehaviour
{
    public UnityEngine.UI.Button btn;
    public UnityEngine.UI.Button next;
    public UnityEngine.UI.Button previous;
    public UnityEngine.UI.Button main;
    public UnityEngine.UI.Text zaman, can, durum;//zaman,can,durum textlerinin kullanımı sağlandı
    float zamanSayaci = 60;
    int canSayaci = 10;
    bool oyunDevam = true;//oyunun devam edip etmediğini kontrol için boolean oluşturuldu
    bool oyunTamam = false;
    Rigidbody fizik;//topun fizik kanunlarına uygun hareket ettirilebilmesi için rigidbody olusturuldu
    AudioSource audio1;
    private bool sessiz;
    bool durdu;
    public GameObject durdurmaMenusu;

    Joystick joystick;

    public float hiz;//hiz değişkeni public olarak oluşturulduğu için unity içerisinde istenen hız değeri verildi.bu değer aşağıda vector ile çarpıldı
    void Start()
    {

        fizik = GetComponent<Rigidbody>();//rigidbody componentine erişim sağlandı
        audio1 = GetComponent<AudioSource>();


        joystick = FindObjectOfType<Joystick>();
    }


    void Update()
    {

        if (oyunDevam && !oyunTamam)
        {
            zamanSayaci -= Time.deltaTime;//zamanSayaci,süre her azaldığında o süreyi zamanSayaci'na ekleyecek
            zaman.text = (int)zamanSayaci + "";//float türündeki zamanSayaci,int'e çevrilip ekranda gösterildi.
        }
        else if (!oyunTamam)
        {
            durum.text = "Bölüm Tamamlanamadı";
            btn.gameObject.SetActive(true);
            main.gameObject.SetActive(true);

        }
        if (zamanSayaci < 0)//zaman sayaci 0'ın altına indiğinde oyunDevam false oldu yani oyun bitti
        {
            oyunDevam = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (durdu)
            {
                durdu = false;
                Time.timeScale = 1f;
                durdurmaMenusu.SetActive(false);


            }
            else
            {

                durdu = true;
                Time.timeScale = 0f;
                durdurmaMenusu.SetActive(true);

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
            durum.text = "Bölüm Tamamlandı,Tebrikler!!";
            btn.gameObject.SetActive(true);
            next.gameObject.SetActive(true);
            previous.gameObject.SetActive(true);
            main.gameObject.SetActive(true);
        }
        else if (!objIsmi.Equals("LabirentZemin") && !objIsmi.Equals("Zemin"))//top,LabirentZemin ve Zemin'e değdiğinde canın azalmaması için şart konuldu
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

    public void DisableAudio()
    {
        SetAudioMute(false);
    }

    public void EnableAudio()
    {
        SetAudioMute(true);
    }

    public void ToggleAudio()
    {
        if (sessiz)
            DisableAudio();
        else
            EnableAudio();
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
    public void TogglePause()
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        durdurmaMenusu.SetActive(true);

    }

    public void AnaMenu()
    {

        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;

    }


    public void oyunaDevam()
    {
        durdu = false;
        Time.timeScale = 1f;
        durdurmaMenusu.SetActive(false);


    }

    public void Yeniden()
    {

        SceneManager.LoadScene("LabirentOyunuLevel9");
        Time.timeScale = 1f;



    }

}
