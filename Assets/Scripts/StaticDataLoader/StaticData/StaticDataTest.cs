using static_data;

public static class StaticDataTest {
    public static TEST_EXCEL_ARRAY Info;

	public static void Init () {
        Info = StaticDataLoader.ReadOneDataConfig<TEST_EXCEL_ARRAY>("test_excel");
    }
}
