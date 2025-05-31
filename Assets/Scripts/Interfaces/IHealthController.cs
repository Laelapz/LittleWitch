using System;
using System.Threading.Tasks;


public interface IHealthController
{
    /// <summary>
    /// Sa�de atual do personagem
    /// </summary>
    public int Health { get; }
    /// <summary>
    /// Evento disparado quando o personagem atualiza a vida.
    /// </summary>
    public static Action<int> OnUpdateLife { get; set; }
    /// <summary>
    /// Evento disparado quando o personagem inicializar a vida.
    /// </summary>
    public static Func<int, Task> OnSetLife { get; set; }
    /// <summary>
    /// Evento disparado quando o personagem recebe dano.
    /// </summary>
    public static Action<int> OnTakeDamage { get; }
    /// <summary>
    /// Evento disparado quando o personagem recupera sa�de.
    /// </summary>
    public static Action<int> OnRecovery { get; }
    /// <summary>
    /// Evento disparado quando o personagem morre.
    /// </summary>
    public static Action OnDie { get; }
    /// <summary>
    /// M�todo respons�vel por inicializar o estado de morte do personagem.
    /// </summary> 
    public Task Die();
    /// <summary>
    /// M�todo respons�vel por recuperar vida do personagem.
    /// </summary>
    /// <param name="amount">Quantidade de vida recuperada</param>
    public void RecoveryLife(int amount);
    /// <summary>
    /// M�todo respons�vel por gerenciar o dano recebido pelo personagem.
    /// </summary>
    /// <param name="damage">Estrutura respons�vel pelos inputs de dano do personagem</param>
    /// <returns></returns>
    public Task<int> TakeDamage(int damage);

    /// <summary>
    /// Metodo responsavel por reiniciar a vida do personagem.
    /// </summary>
    public void Restore();
    public void SetupLife();
}

