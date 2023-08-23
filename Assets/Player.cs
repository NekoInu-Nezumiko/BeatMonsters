using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;

//JSON�p�ɃV���A����
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
    //�R�C���e�L�X�g
    public TextMeshProUGUI coinLabel;
    //���x���e�L�X�g
    public TextMeshProUGUI levellabel;
    //�R�C������
    public int coin;
    //���݂̃��x��
    public int level;
    //�G��|������
    public int kill;
    //�p���[�A�b�v�{�^��
    public Button powerUpBtn;
    //�p���[�A�b�v�ɕK�v�ȃR�C���̐�
    public TextMeshProUGUI powerUpCoinText;

    public void setParam(int coin, int level, int kill)
    {
        this.coin = coin;
        this.level = level;
        this.kill = kill;
    }

    //�p���[�A�b�v�ɕK�v�ȃR�C�����v�Z
    public int PowerUpCoin()
    {
        return (level - 1) * 20 + 100;
    }
    public void UpdateUI()
    {
        coinLabel.text = "Coin: " + coin;
        levellabel.text = "Level: " + level;

        //UI�A�b�v�f�[�g����PowerUpCoin�֐����Ăяo��
        int require = PowerUpCoin();
        if (coin < require)
        {
            powerUpBtn.interactable = false;//interactable�Ń{�^���̗L��/����
        }
        else
        {
            powerUpBtn.interactable = true;
        }
        //�K�v�ȃR�C���̐����{�^���̃e�L�X�g�ɕ\��
        powerUpCoinText.text = "Coin: " + require;
    }

    //���̃X�N���v�g��������̕ϐ����g���̂�public
    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateUI();
    }

    //PowerUp�{�^�����������Ƃ��̏���
    public void OnPowerUp()
    {
        int require = PowerUpCoin();
        if (coin >= require)
        {
            level++;
            AddCoin(-require);
        }
    }

    //Save�u���b�N���N���b�N�����Ƃ���Save(�������������ł���悤�ɂȂ������O�̂��߃��[�h���Ă�����
    public void OnSave()
    {
        //ChClass��JSON��ۑ�
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
        //OS�Ԃ̃t�@�C���\�����z������
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        //���݂��Ȃ��ꍇ�A�������ݏ������K�v
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

