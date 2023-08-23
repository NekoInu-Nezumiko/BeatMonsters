using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;

//JSON用にシリアル化
[System.Serializable]
public class ChClass
{
    public int coin;
    public int level = 1;
    public int kill;

    public void setParam(int coin, int level, int kill)
    {
        this.coin = coin;
        this.level = level;
        this.kill = kill;
    }
}


public class Player : MonoBehaviour
{
    //file_path
    private string filePath;
    private string fileName = "beatMonsters.json";
    //コインテキスト
    public TextMeshProUGUI coinLabel;
    //レベルテキスト
    public TextMeshProUGUI levellabel;
    //コイン枚数
    public int coin;
    //現在のレベル
    public int level;
    //敵を倒した数
    public int kill;
    //パワーアップボタン
    public Button powerUpBtn;
    //パワーアップに必要なコインの数
    public TextMeshProUGUI powerUpCoinText;

    public void setParam(int coin, int level, int kill)
    {
        this.coin = coin;
        this.level = level;
        this.kill = kill;
    }

    //パワーアップに必要なコインを計算
    public int PowerUpCoin()
    {
        return (level - 1) * 20 + 100;
    }
    public void UpdateUI()
    {
        coinLabel.text = "Coin: " + coin;
        levellabel.text = "Level: " + level;

        //UIアップデート時にPowerUpCoin関数を呼び出す
        int require = PowerUpCoin();
        if (coin < require)
        {
            powerUpBtn.interactable = false;//interactableでボタンの有効/無効
        }
        else
        {
            powerUpBtn.interactable = true;
        }
        //必要なコインの数をボタンのテキストに表示
        powerUpCoinText.text = "Coin: " + require;
    }

    //他のスクリプトからもこの変数を使うのでpublic
    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateUI();
    }

    //PowerUpボタンを押したときの処理
    public void OnPowerUp()
    {
        int require = PowerUpCoin();
        if (coin >= require)
        {
            level++;
            AddCoin(-require);
        }
    }

    //SaveブロックをクリックしたときにSave(けっこう早くできるようになったが念のためロードしてもいい
    public void OnSave()
    {
        //ChClassでJSONを保存
        ChClass save = new ChClass();
        save.setParam(this.coin, this.level, this.kill);
        Save(save);
    }
    // Start is called before the first frame update
    void Start()
    {
        Load();
        UpdateUI();
    }

    public ChClass Load()
    {
        //OS間のファイル構造を吸収する
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        //存在しない場合、書き込み処理が必要
        if (!File.Exists(filePath))
        {
            OnSave();
        }
        var json = File.ReadAllText(filePath);
        ChClass data = JsonUtility.FromJson<ChClass>(json);
        setParam(data.coin, data.level, data.kill);

        Debug.Log(data + ":load");
        return data;

    }
    public void Save(ChClass ins)
    {
        string json = JsonUtility.ToJson(ins, true);
        Debug.Log(json + ":save");
        File.WriteAllText(filePath, json);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

