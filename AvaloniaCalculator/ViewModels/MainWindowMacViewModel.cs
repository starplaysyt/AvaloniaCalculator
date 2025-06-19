using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Input;
using AvaloniaCalculator.Models;
using HarfBuzzSharp;
using ReactiveUI;

namespace AvaloniaCalculator.ViewModels;

public class MainWindowMacViewModel : ViewModelBase
{
   private int _numericId;
   private string _inputText = "0";
   private string _listOfOperations = "0";
   private KeyEventArgs? _lastPressedArgs = null;
   private int _memoryNumber = 0;

   public int MaxSymbols = 15;
   
   public Calculator Calculator = new Calculator();

   public string InputText
   {
      get => _inputText;
      set
      {
         string result = value;
         if (value.Length > MaxSymbols)
         {
            result = value.Substring(0, MaxSymbols);
         }
         this.RaiseAndSetIfChanged(ref _inputText, result);
      } 
   }
   public int NumericId
   {
      get => _numericId;
      set => this.RaiseAndSetIfChanged(ref _numericId, value);
   }
   public string ListOfOperations
   {
      get => _listOfOperations;
      set => this.RaiseAndSetIfChanged(ref _listOfOperations, value);
   }
   public KeyEventArgs? LastPressedArgs
   {
      get => _lastPressedArgs;
      set => this.RaiseAndSetIfChanged(ref _lastPressedArgs, value);
   }

   
   public ReactiveCommand<Unit, Unit> HandleKeysCommand { get; }
   public ReactiveCommand<Unit, Unit> NumericButtonClickCommand { get; }
   public ReactiveCommand<Unit, Unit> MinusButtonClickCommand { get; }
   public ReactiveCommand<Unit, Unit> PlusButtonClickCommand { get; }
   public ReactiveCommand<Unit, Unit> MultiplyButtonClickCommand { get; }
   public ReactiveCommand<Unit, Unit> DivideButtonClickCommand { get; }
   public ReactiveCommand<Unit, Unit> ResultClickCommand { get; }
   public ReactiveCommand<Unit, Unit> ClearValueCommand { get; }
   public ReactiveCommand<Unit, Unit> ClearAllCommand { get; }
   
   public ReactiveCommand<Unit, Unit> MemorySetCommand { get; }
   
   public ReactiveCommand<Unit, Unit> MemoryPasteCommand { get; }
   
   public MainWindowMacViewModel()
   {
      HandleKeysCommand = ReactiveCommand.CreateFromTask(HandleKeys);
      NumericButtonClickCommand = ReactiveCommand.CreateFromTask(NumericButtonClick);
      MinusButtonClickCommand = ReactiveCommand.Create(MinusButtonClick);
      PlusButtonClickCommand = ReactiveCommand.Create(PlusButtonClick);
      MultiplyButtonClickCommand = ReactiveCommand.Create(MultiplyButtonClick);
      DivideButtonClickCommand = ReactiveCommand.Create(DivideButtonClick);
      ResultClickCommand = ReactiveCommand.CreateFromTask(ResultClick);
      ClearValueCommand = ReactiveCommand.Create(ClearValue);
      ClearAllCommand = ReactiveCommand.Create(ClearAll);
      MemoryPasteCommand = ReactiveCommand.Create(PasteMemory);
      MemorySetCommand = ReactiveCommand.Create(SetMemory);
   }

   private void MoveAndIntercept(char oper)
   {
      if (ListOfOperations == "0") ListOfOperations = "";
      ListOfOperations += InputText + " " + oper + " ";
      InputText = "0";
   } 
   private void ClearValue() => InputText = "0";
   private void ClearAll() => ListOfOperations = InputText = "0";
   private void MinusButtonClick() => MoveAndIntercept('-');
   private void PlusButtonClick() => MoveAndIntercept('+');
   private void MultiplyButtonClick() => MoveAndIntercept('*');
   private void DivideButtonClick() => MoveAndIntercept('/');

   private Task ResultClick()
   {
      InputText = Calculator.Evaluate(ListOfOperations + InputText);
      ListOfOperations = "0";
      return Task.CompletedTask;
   }
   
   private Task HandleKeys()
   {
      if (LastPressedArgs is null)
         return Task.CompletedTask;

      switch (LastPressedArgs.KeySymbol)
      {
         case "+": PlusButtonClick(); break;
         case "-": MinusButtonClick(); break;
         case "*": MultiplyButtonClick(); break;
         case "/": DivideButtonClick(); break;
         case "=": ResultClick(); break;
      }

      if (LastPressedArgs.Key is >= Key.D0 and <= Key.D9)
      {
         int res = 0;
         if (int.TryParse(LastPressedArgs.KeySymbol, out res))
         {
            NumericId = res;
            NumericButtonClick();
         }
      }

      if (LastPressedArgs.Key == Key.Back)
      {
         if (InputText == "0") return Task.CompletedTask;
         InputText = InputText.Remove(InputText.Length - 1);
         if (InputText == "") InputText = "0";
      }

      if (LastPressedArgs.Key == Key.Enter)
      {
         ResultClick();
      }
      return Task.CompletedTask;
   }
   private Task NumericButtonClick()
   {
      if (InputText == "0") InputText = "";
      InputText += NumericId;
      return Task.CompletedTask;
   }

   private void SetMemory()
   {
      if (int.TryParse(InputText, out var newMem))
         _memoryNumber = newMem;
   }

   private void PasteMemory()
   {
      InputText = _memoryNumber.ToString();
   }
}