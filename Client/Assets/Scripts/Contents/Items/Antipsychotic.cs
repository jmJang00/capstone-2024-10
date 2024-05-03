using DG.Tweening;
using UnityEngine;

public class Antipsychotic : BaseItem
{
    public override void SetInfo(int templateId, Crew owner)
    {
        base.SetInfo(templateId, owner);

        ItemGameObject.transform.localPosition = new Vector3(0.02f, 0.01f, -0.043f);
        ItemGameObject.transform.localEulerAngles = new Vector3(-9f, -83f, 83f);
        ItemGameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        ItemGameObject.SetActive(false);
    }
    public override bool CheckAndUseItem()
    {
        if (Owner.CrewStat.Sanity >= Owner.CrewStat.MaxSanity)
            return false;

        UseItem();
        return true;
    }

    protected override void UseItem()
    {
        DOVirtual.Float(0, 0, 5, value =>
        {
            Owner.CrewStat.ChangeSanity(ItemData.Value * Time.deltaTime);
        });
    }
}