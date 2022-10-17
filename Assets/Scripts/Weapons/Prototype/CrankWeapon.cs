using UnityEngine;
using TMPro;

public class CrankWeapon : AmmoWeapon
{
    [SerializeField] private ConfigurableJoint joint;
    [SerializeField] private float turnsPerBullet = 1;
    [SerializeField] private TextMeshPro text;

    private float turns;
    private float lastRotation;

    private new void OnEnable()
    {
        base.OnEnable();
    }

    protected override void UpdateBulletDisplay() => text.text = bullets.ToString();

    public void Update()
    {
        var rotation = joint.Angles().x;

        turns += Mathf.Max(0, (rotation - lastRotation) % 180f) / 360;

        if (turns >= turnsPerBullet)
        {
            turns -= turnsPerBullet;

            if (bullets < maxBullets.Value)
            {
                AddBullet();
                text.text = bullets.ToString();
            }
        }

        lastRotation = rotation;
    }
}
