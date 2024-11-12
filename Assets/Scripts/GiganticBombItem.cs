using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticBombItem : ItemBase
{
   //大爆発itemの設定
    [SerializeField]
    Explosion giganticExplosionPrefab_;

    public override void Get()
    {
        Instantiate(
            giganticExplosionPrefab_,
            transform.position,
            Quaternion.identity
        );
        Destroy(this.gameObject);
    }
}
