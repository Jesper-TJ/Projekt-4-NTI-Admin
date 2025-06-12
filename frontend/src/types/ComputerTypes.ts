export interface IComputer {
  id: number;
  name: string;
  serial: string;
  status: string;
  damage: string;
  entry_date: string;
}

export interface IComputerlog {
  id: number;
  user_id: number;
  user_name: string;
  computer_id: number;
  return_date: string;
  entry_date: string;
}
