using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.UI
{
    public class ListItemExtensions<T> : MonoBehaviour where T: MonoBehaviour
    {
        [SerializeField] protected T itemPrefab;
        [SerializeField] protected Transform holder;
        [SerializeField] protected List<T> listItem;

        public void FixSize(int amount)
        {
            if (listItem == null)
            {
                listItem = new List<T>();
            }
            for (int i = 0; i < amount; i++)
            {
                if (listItem.Count <= i)
                {
                    var newItem = Instantiate(itemPrefab, holder);
                    newItem.gameObject.SetActive(true);
                    listItem.Add(newItem);
                }
                else
                {
                    listItem[i].gameObject.SetActive(true);
                }
            }
            if (listItem.Count > amount)
            {
                for (int i = amount; i < listItem.Count; i++)
                {
                    listItem[i].gameObject.SetActive(false);
                }
            }
        }
    }
}