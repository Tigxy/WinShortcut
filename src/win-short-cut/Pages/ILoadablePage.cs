namespace win_short_cut.Pages {
    public interface ILoadablePage
    {
        public void LoadPage();
        public void LoadPage(params object[] parameters);
    }
}
