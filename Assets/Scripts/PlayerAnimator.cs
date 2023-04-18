using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerAnimator : NetworkBehaviour {
   
    private const string IS_WALKING = "IsWalking";
    [SerializeField] private Player player;
    private Animator animator;

    // Start is called before the first frame update
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update() {
        if(!IsOwner) {
            return;
        }

        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
