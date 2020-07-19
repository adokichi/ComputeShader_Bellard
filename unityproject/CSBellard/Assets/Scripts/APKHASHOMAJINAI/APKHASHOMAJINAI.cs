using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APKHASHOMAJINAI : MonoBehaviour
{
    //apkﾌｧｲﾙとソースコードからNCMBSettingsのkeyが復元されたら困るのでさらに意味のない暗号をかます(これが意味があるかもわからんが)
    [SerializeField]Text text;
    public string mykey;
    int cnt = 0;
    void Start()
    {
        text.text = mykey;
    }

    private void Update()
    {
        if (cnt == 3)
            Destroy(this.gameObject);
        cnt++;
    }

}
