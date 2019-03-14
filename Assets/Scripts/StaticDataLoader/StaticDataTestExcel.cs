using UnityEngine;
using static_data;
using System.Text;

public class StaticDataTestExcel : MonoBehaviour {

    private TEST_EXCEL_ARRAY infos;
    public TEST_EXCEL_ARRAY Infos {
        get { return infos; }
    }

	void Start () {
        infos = StaticDataLoader.ReadOneDataConfig<TEST_EXCEL_ARRAY>("test_excel");
        Debug.Log(infos.items[0].id);
        Debug.Log(Encoding.UTF8.GetString(infos.items[0].name));
    }
}
