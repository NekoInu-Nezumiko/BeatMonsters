using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class Monster : MonoBehaviour
{
    //攻撃エフェクト
    public GameObject hitEffect; //コンテナ
    //コインエフェクト
    public GameObject coinEffect;
    //コインの効果音
    public AudioClip coinSE;
    //オーディオソース
    public AudioSource audioSource;


    //プレイヤー情報
    public Player player;
    //最低レベル&最高レベル&最大レベル
    private int levelMinHp = 20;
    private int levelMaxHp = 10000;
    private int maxLevel = 100;
    //指数
    private float degree = 1.2f;
    //コイン獲得倍率
    private float coinMultiplier = 0.5f;

    //HPテキスト
    public TextMeshProUGUI hpLabel;
    private int hp;
    private int maxHp;

    //モンスターランダム出現
    //画像切り替え(アタッチする)
    public Image monsterImage;
    public Sprite[] monsterImages;

    //モンスターのHP計算
    private int CalcHp()
    {
        float tmp = Mathf.Pow((float)player.kill / maxLevel, degree);
        int hp = (int)((levelMaxHp - levelMinHp) * tmp + levelMinHp + 0.5f);

        return hp;
    }
    //モンスター初期化
    private void SetUp()
    {
        //hpLabel = GetComponent<TextMeshProUGUI>();
        maxHp = CalcHp();
        hp = maxHp;
        hpLabel.text = hp + " / " + maxHp;

        int imageIndex = Random.Range(0, monsterImages.Length);
        monsterImage.sprite = monsterImages[imageIndex];
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    //ボタン用のイベント
    public void OnClickMonster()
    {
        //damage
        hp -= player.level;
        hpLabel.text = hp + " / " + maxHp;
        //hitエフェクト
        //プレハブをinstantiate化
        GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(hit, 0.5f);

        //enemy_death
        if (hp <= 0)
        {
            audioSource.PlayOneShot(coinSE); //コインの音を一度だけ鳴らす
            GameObject coin = Instantiate(coinEffect,transform.position, Quaternion.identity);
            Destroy(coin, 3f);
            player.kill++;
            //Hpからcointの枚数を出し、加算
            int amount = (int)(CalcHp() * coinMultiplier);
            player.AddCoin(amount);
            SetUp();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
