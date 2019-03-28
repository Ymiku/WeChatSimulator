using static_data;

public static class StaticDataTest {
    public static TEST_EXCEL_ARRAY Infos {
        get { return Infos; }
        private set { Infos = value; }
    }

	public static void Init () {
        Infos = StaticDataLoader.ReadOneDataConfig<TEST_EXCEL_ARRAY>("test_excel");
    }
}
