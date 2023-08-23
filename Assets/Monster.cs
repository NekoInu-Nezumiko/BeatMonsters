using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class Monster : MonoBehaviour
{
    //�U���G�t�F�N�g
    public GameObject hitEffect; //�R���e�i
    //�R�C���G�t�F�N�g
    public GameObject coinEffect;
    //�R�C���̌��ʉ�
    public AudioClip coinSE;
    //�I�[�f�B�I�\�[�X
    public AudioSource audioSource;


    //�v���C���[���
    public Player player;
    //�Œ჌�x��&�ō����x��&�ő僌�x��
    private int levelMinHp = 20;
    private int levelMaxHp = 10000;
    private int maxLevel = 100;
    //�w��
    private float degree = 1.2f;
    //�R�C���l���{��
    private float coinMultiplier = 0.5f;

    //HP�e�L�X�g
    public TextMeshProUGUI hpLabel;
    private int hp;
    private int maxHp;

    //�����X�^�[�����_���o��
    //�摜�؂�ւ�(�A�^�b�`����)
    public Image monsterImage;
    public Sprite[] monsterImages;

    //�����X�^�[��HP�v�Z
    private int CalcHp()
    {
        float tmp = Mathf.Pow((float)player.kill / maxLevel, degree);
        int hp = (int)((levelMaxHp - levelMinHp) * tmp + levelMinHp + 0.5f);

        return hp;
    }
    //�����X�^�[������
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

    //�{�^���p�̃C�x���g
    public void OnClickMonster()
    {
        //damage
        hp -= player.level;
        hpLabel.text = hp + " / " + maxHp;
        //hit�G�t�F�N�g
        //�v���n�u��instantiate��
        GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(hit, 0.5f);

        //enemy_death
        if (hp <= 0)
        {
            audioSource.PlayOneShot(coinSE); //�R�C���̉�����x�����炷
            GameObject coin = Instantiate(coinEffect,transform.position, Quaternion.identity);
            Destroy(coin, 3f);
            player.kill++;
            //Hp����coint�̖������o���A���Z
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
