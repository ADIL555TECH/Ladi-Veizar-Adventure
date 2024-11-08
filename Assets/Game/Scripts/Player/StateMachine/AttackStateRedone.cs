using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateRedone : State
{
    PlayerController player;
    PlayerAvatar avatar;

    List<PlayerAttack> attackChain;

    GameObject attackCollider;
    int attackStage = -1; 

    float attackChainWindow = .3f;

    float startTime;
    //float DurationTime;
    float antecipationTime;
    float attackCoolDown;
    bool applyImpulse = false;

    float effectDuration = 0.6f;
    bool isEffectsActive = false;

    bool waitForInput= false;
    
    bool antecipation = false;
    bool duration = false;
    bool cooldown = false;
    float lastedtime = 0f;
    public AttackStateRedone(PlayerController playerController) : base("Attack")
    {
        player = playerController;
        avatar = player.GetControlledAvatar();
    }

    public void SetAttackChain(List<PlayerAttack> attackChain, GameObject attackCollider)
    {
        this.attackChain = attackChain;
        this.attackCollider =attackCollider;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        avatar.Animator.SetBool("bIsAttacking", true);
        attackStage = 0;
        attackCollider.SetActive(true);
        StartAttack();
        SetAttackTimers();
    }
    public override void OnStateExit()
    {
        base.OnStateExit();
       attackCollider.SetActive(false);
       avatar.Animator.SetBool("bIsAttacking", false);
       player.exitiAttackTime = Time.time+attackCoolDown;
    }
    public override void OnStateUpdate()
    {
        AttackRountineVer1();
        //AttackRountineVer2();
    }

    private void AttackRountineVer2()
    {
        
        base.OnStateUpdate();
        // wait atecipation
        if(!antecipation)
        {
            if(Time.time< startTime+antecipationTime) return;
            antecipation = true;
            Debug.Log("antecipation");
        }
        
        
        // trigger effect
        if (!isEffectsActive)
        {
            TriggerAttackEffects(attackStage);
            startTime = Time.time;
            return;
        } 
        
        //wait duration
        if(!duration)
        {
            if(Time.time< startTime+effectDuration) return;
            Debug.Log("Duration");
            startTime = Time.time;
            duration = true;
            //disable effect
            if (isEffectsActive)
            {
                lastedtime = Time.time - lastedtime;
                Debug.Log(lastedtime);
                attackCollider.SetActive(false);
                isEffectsActive = false;
            }
            return;
        }
         //if not input leave
        if (!player.ReadAttackInput())
            {
                player.StateMachine.ChangeState(player.IdleState);
                return;
            }
        // wait cooldown
        if(!cooldown)
        {
            if(Time.time<startTime+attackCoolDown) return;
            Debug.Log("Cooldown");
            cooldown = true;
            startTime = Time.time;
            //read input 
            if (player.ReadAttackInput())
                {
                    EvolveAttackStages();
                    StartAttack();
                    return;
                }
        }    
    }

    private void AttackRountineVer1()
    {
        base.OnStateUpdate();
        // se excedeu a duração voltar para idle;
        if (Time.time > startTime + antecipationTime + effectDuration + attackCoolDown + attackChainWindow)
        {
            player.StateMachine.ChangeState(player.IdleState);
            return;
        }
        // espera antencipação
        if (Time.time < startTime + antecipationTime) return;
        // aplicar efeitos após tempo de antecipação
        if (Time.time < startTime + antecipationTime + effectDuration)
            if (!isEffectsActive) TriggerAttackEffects(attackStage);

        // espera o tempo de efeito para desliga-lo e ler inputs
        if (Time.time > startTime + antecipationTime + effectDuration)
        {
            // destivar efeitos se o tempo de efeito ja passou
            if (isEffectsActive)
            {
                
                attackCollider.SetActive(false);
                isEffectsActive = false;
            }
            //cancelar se jogador se movimenta
            if (!player.ReadAttackInput())
            {
                player.StateMachine.ChangeState(player.IdleState);
                return;
            }
        }
        // após o cooldown abrir input do jogador
        if (Time.time > startTime + antecipationTime + effectDuration + attackCoolDown)
        {

            // abrir janela de encadeamento de ataques
            if (player.ReadAttackInput())
            {
                EvolveAttackStages();
                StartAttack();
                return;
            }
            return;
        }
    }

    private void StartAttack()
    {
        player.GetControlledAvatar().PlayAttackAnimation(attackChain[attackStage].GetAttackStats().AttackTriggerTag);
        player.GetControlledAvatar().mySFXManager.PlayAudio();
        SetAttackTimers();
    }

    public void TriggerAttackEffects(int stage)
    {
        Debug.Log("triggerer");
        isEffectsActive = true;
        //turn on colliders
        lastedtime = Time.time;
        attackCollider.SetActive(true);
        applyImpulse = true;
        //add impulse
        
    }
    public void EvolveAttackStages()
    {
        attackStage++;
        Debug.Log("went attack stage:"+attackStage);
        if (attackStage >= attackChain.Count) attackStage = 0;
        player.GetControlledAvatar().attackstage = attackStage;
    }

    private void SetAttackTimers()
    {
        PlayerAttackStruct attackStats= attackChain[attackStage].GetAttackStats();
        startTime = Time.time;
        antecipationTime = attackStats.attackPreparationTime;
        effectDuration = attackStats.attackDuration;
        attackCoolDown = attackStats.attackCooldown;

        antecipation = false;
        duration = false;
        cooldown = false;

        
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        player.GetControlledAvatar().onRotate?.Invoke(1);
        if(applyImpulse) 
        {
            player.GetControlledAvatar().onPlayerImpulse(attackChain[attackStage].GetAttackStats().AttackImpulse);
            //player.PlayAttackImpulse(attackStage);
            applyImpulse = false;
        }
    }   
    public override void OnStateLateUpdate()
    {
        base.OnStateLateUpdate();
    }

    IEnumerator TriggerEffectsForATime(float duration)
    {
        StartAttack();
        yield return new WaitForSeconds(antecipationTime);
        TriggerAttackEffects(attackStage);
        yield return new WaitForSeconds(effectDuration);
        attackCollider.SetActive(false);
        isEffectsActive = false;
        yield return new WaitForSeconds(attackCoolDown);

    }
}
