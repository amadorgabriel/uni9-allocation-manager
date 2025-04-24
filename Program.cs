using System;
using System.Linq;
using System.Collections.Generic;

public abstract class Funcionario

{
	public string Nome { get; private set; }
	public string Cargo { get; protected set; }

	public Funcionario(string nome)
	{
		Nome = nome;
	}

	public abstract double CalcularCustoHora();
}


public class Desenvolvedor : Funcionario
{
	public Desenvolvedor(string nome) : base(nome)
	{
		Cargo = "Desenvolvedor";
	}

	public override double CalcularCustoHora() => 100.0;
}


public class Gerente : Funcionario

{
	public Gerente(string nome) : base(nome)
	{
		Cargo = "Gerente";
	}

	public override double CalcularCustoHora() => 150.0;
}

public class Projeto
{
	public string Nome { get; private set; }
	public List<Alocacao> Alocacoes { get; private set; } = new List<Alocacao>();
	public Projeto(string nome)
	{
		Nome = nome;
	}


	public void AdicionarAlocacao(Funcionario funcionario, int horas)
	{

		if (horas <= 0)
		{
			throw new ArgumentException("Horas devem ser maiores que zero.");
		}

		Alocacoes.Add(new Alocacao(funcionario, horas));

	}

	public double CalcularCustoTotal()
	{
		return Alocacoes.Sum(a => a.Horas * a.Funcionario.CalcularCustoHora());
	}

	public void ExibirDetalhes()
	{
		Console.WriteLine($"Projeto: {Nome}");

		foreach (var aloc in Alocacoes)
		{
			Console.WriteLine($"- {aloc.Funcionario.Cargo}: {aloc.Funcionario.Nome} ({aloc.Horas}h)");
		}

		Console.WriteLine($"Custo total: R${CalcularCustoTotal():F2}");
	}
}

public class Alocacao
{
	public Funcionario Funcionario { get; private set; }

	public int Horas { get; private set; }

	public Alocacao(Funcionario funcionario, int horas)
	{
		Funcionario = funcionario;
		Horas = horas;
	}
}

public class Repositorio
{

	public List<Funcionario> Funcionarios { get; private set; } = new List<Funcionario>();

	public List<Projeto> Projetos { get; private set; } = new List<Projeto>();

	public void AdicionarFuncionario(Funcionario f) => Funcionarios.Add(f);

	public void AdicionarProjeto(Projeto p) => Projetos.Add(p);

	public Funcionario BuscarFuncionario(string nome) => Funcionarios.FirstOrDefault(f => f.Nome.ToLower() == nome.ToLower());

	public Projeto BuscarProjeto(string nome) => Projetos.FirstOrDefault(p => p.Nome.ToLower() == nome.ToLower());
}


internal class Program
{
	static Repositorio repo = new Repositorio();

	static void Main()
	{
		bool sair = false;

		while (!sair)
		{
			Console.WriteLine("\n=== MENU ===");

			Console.WriteLine("1 - Cadastrar FuncionC!rio");

			Console.WriteLine("2 - Cadastrar Projeto");

			Console.WriteLine("3 - Alocar FuncionC!rio a Projeto");

			Console.WriteLine("4 - Exibir Projetos");

			Console.WriteLine("5 - Sair");

			Console.Write("Escolha: ");

			var opcao = Console.ReadLine();

			switch (opcao)
			{

				case "1":
					CadastrarFuncionario();
					break;

				case "2":
					CadastrarProjeto();
					break;

				case "3":
					AlocarFuncionario();
					break;

				case "4":
					ExibirProjetos();
					break;

				case "5":
					sair = true;
					break;

				default:
					Console.WriteLine("OpC'C#o invC!lida.");
					break;
			}
		}
	}

	static void CadastrarFuncionario()
	{

		Console.Write("Nome: ");

		var nome = Console.ReadLine();

		Console.Write("Cargo (1 - Desenvolvedor | 2 - Gerente): ");

		var cargo = Console.ReadLine();

		Funcionario f = cargo == "1"
						? (Funcionario)new Desenvolvedor(nome)
						: new Gerente(nome);

		repo.AdicionarFuncionario(f);

		Console.WriteLine("FuncionC!rio cadastrado!");
	}

	static void CadastrarProjeto()
	{
		Console.Write("Nome do projeto: ");

		var nome = Console.ReadLine();

		repo.AdicionarProjeto(new Projeto(nome));

		Console.WriteLine("Projeto cadastrado!");
	}

	static void AlocarFuncionario()
	{
		Console.Write("Nome do projeto: ");

		var nomeProjeto = Console.ReadLine();

		var projeto = repo.BuscarProjeto(nomeProjeto);

		if (projeto == null)
		{
			Console.WriteLine("Projeto nC#o encontrado.");
			return;
		}

		Console.Write("Nome do funcionC!rio: ");

		var nomeFuncionario = Console.ReadLine();

		var funcionario = repo.BuscarFuncionario(nomeFuncionario);

		if (funcionario == null)
		{
			Console.WriteLine("FuncionC!rio nC#o encontrado.");
			return;
		}

		Console.Write("Horas alocadas: ");

		int horas = int.Parse(Console.ReadLine());

		projeto.AdicionarAlocacao(funcionario, horas);

		Console.WriteLine("AlocaC'C#o realizada!");
	}

	static void ExibirProjetos()
	{
		foreach (var p in repo.Projetos)
		{
			p.ExibirDetalhes();
			Console.WriteLine("-----------");
		}
	}
}