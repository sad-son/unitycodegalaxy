using System;

[Serializable]
public struct Question : IEquatable<Question>
{
    public string title;
    public string short_title;
    public Answer[] answers;

    public bool Equals(Question other)
    {
        return title.GetHashCode() == other.title.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return obj is Question other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(title, short_title, answers);
    }
}