using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Database", menuName = "Characters/Database")]
public class Characters : ScriptableObject
{
    [SerializeField] ClassSheet[] characters = new ClassSheet[0];

    public ClassSheet[] GetAllCharacters() => characters;

    public ClassSheet GetCharacterById(int id)
    {
        foreach (var character in characters)
        {
            if (character.Id == id)
            {
                return character;
            }
        }

        return null;
    }

    public bool IsCharacterIdValid(int id)
    {
        return characters.Any(x => x.Id == id);
    }
}
