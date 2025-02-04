using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class SubjectChapter : MonoBehaviour
    {
        public Dictionary<string, List<Question>> _subjects;
        public TMP_Text _text;
        
        public void Setup(string subject)
        {
            _text.text = subject;
        }
    }
}