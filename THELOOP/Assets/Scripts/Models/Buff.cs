[System.Serializable]
public class Buff
{
    public string title;
    public string description;
    public int tag;

    public Buff(string title, string description, int tag)
    {
        this.title = title;
        this.description = description;
        this.tag = tag;
    }
}
