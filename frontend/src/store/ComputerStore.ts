import { defineStore } from "pinia";
import { IComputer, IComputerlog } from "@/types/ComputerTypes";

export const useComputerStore = defineStore("ComputerStore", {
  state: () => {
    return {
      Computers: [] as { computer: IComputer; computerlog: IComputerlog }[],
      Computer: [] as { computer: IComputer; computerlog: IComputerlog }[],
    };
  },
  actions: {
    async fetchComputers() {
      try {
        const response = await fetch("https://localhost:3001/api/computers/all");

        if (response.ok) {
          const data = await response.json();

          // Ensure the data is in the expected format
          if (Array.isArray(data) && data.length > 0) {
            const formattedData = data
              .filter((item) => {
                if (item && item.computer && item.computerlog) {
                  return true;
                }
                else {
                  console.warn("Invalid data structure:", item);
                  return false;
                }
              })
              .map((item) => {
                return {
                  computer: this.convertToComputer(item.computer),
                  computerlog: this.convertToComputerlog(item.computerlog),
                };
              })

            // Store the formatted data if valid
            if (formattedData.length > 0) {
              this.Computers = formattedData;
              return;
            } else {
              console.warn("No valid data found, using default values.");
            }
          }
        } else {
          console.warn("API call failed, using default values.");
        }
      } catch (error) {
        console.error("Failed to fetch computers:", error);
      }

      this.Computers = jsonStuff;
    },

    // Convert raw JSON data to IComputer type
    convertToComputer(rawData: any): IComputer {
      console.log(`Recieved data ${rawData}`)
      return {
        serial: rawData.serial,
        name: rawData.name,
        status: rawData.status,
        damage: rawData.damage,
        entry_date: rawData.entryDate,
      } as IComputer;
    },

    // Convert raw JSON data to IComputerlog type
    convertToComputerlog(rawData: any): IComputerlog {
      console.log(`Recieved data ${rawData}`)
      return {
        id: rawData.id,
        user_id: rawData.userId,
        user_name: rawData.userName,
        computer_id: rawData.computerId,
        return_date: rawData.returnDate,
        entry_date: rawData.entryDate,
      } as IComputerlog;
    },
  },
  getters: {
    allComputers(): { computer: IComputer; computerlog: IComputerlog }[] {
      return this.Computers;
    },
    getComputer:
      (state) =>
      (
        computer_id: string,
      ): { computer: IComputer; computerlog: IComputerlog } | undefined => {
        return state.Computers.find(
          (computer) => computer.computer.serial === computer_id,
        );
      },
  },
});

const jsonStuff = [
  {
    computer: {
      serial: "123456789",
      name: "Airbook 13",
      status: "In Use",
      damage: "Fine",
      entry_date: "2021-10-01",
    } as IComputer,
    computerlog: {
      id: 1,
      user_id: 1,
      user_name: "Kevin Radley",
      computer_id: 1,
      return_date: "2023-12-31", // Set to a future date
      entry_date: "2021-10-01",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "987654321",
      name: "Zenbook Pro",
      status: "Returned",
      damage: "Minor scratches",
      entry_date: "2021-09-15",
    } as IComputer,
    computerlog: {
      id: 2,
      user_id: 2,
      user_name: "Sarah Connor",
      computer_id: 2,
      return_date: "2021-10-01",
      entry_date: "2021-09-15",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "111223344",
      name: "ThinkPad X1",
      status: "Overdue",
      damage: "Screen cracked",
      entry_date: "2021-08-20",
    } as IComputer,
    computerlog: {
      id: 3,
      user_id: 3,
      user_name: "John Doe",
      computer_id: 3,
      return_date: "2021-09-20",
      entry_date: "2021-08-20",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "555666777",
      name: "Macbook Pro 14",
      status: "In Use",
      damage: "Fine",
      entry_date: "2022-01-01",
    } as IComputer,
    computerlog: {
      id: 4,
      user_id: 4,
      user_name: "Alice Smith",
      computer_id: 4,
      return_date: "2023-12-31", // Set to a future date
      entry_date: "2022-01-01",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "888999000",
      name: "Dell Inspiron",
      status: "Returned",
      damage: "Fine",
      entry_date: "2022-03-01",
    } as IComputer,
    computerlog: {
      id: 5,
      user_id: 5,
      user_name: "Bob Johnson",
      computer_id: 5,
      return_date: "2022-04-01",
      entry_date: "2022-03-01",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "000999888",
      name: "HP Spectre x360",
      status: "Overdue",
      damage: "Battery issue",
      entry_date: "2022-02-15",
    } as IComputer,
    computerlog: {
      id: 6,
      user_id: 6,
      user_name: "Emily Davis",
      computer_id: 6,
      return_date: "2022-03-15",
      entry_date: "2022-02-15",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "333444555",
      name: "Surface Pro 8",
      status: "In Use",
      damage: "Keyboard issue",
      entry_date: "2022-04-01",
    } as IComputer,
    computerlog: {
      id: 7,
      user_id: 7,
      user_name: "Michael Brown",
      computer_id: 7,
      return_date: "2023-12-31", // Set to a future date
      entry_date: "2022-04-01",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "666777888",
      name: "Lenovo Yoga 9i",
      status: "Returned",
      damage: "Fine",
      entry_date: "2021-07-15",
    } as IComputer,
    computerlog: {
      id: 8,
      user_id: 8,
      user_name: "Olivia Wilson",
      computer_id: 8,
      return_date: "2021-08-15",
      entry_date: "2021-07-15",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "112233445",
      name: "Acer Aspire 5",
      status: "Overdue",
      damage: "Hard drive issue",
      entry_date: "2021-05-20",
    } as IComputer,
    computerlog: {
      id: 9,
      user_id: 9,
      user_name: "James Lee",
      computer_id: 9,
      return_date: "2021-06-20",
      entry_date: "2021-05-20",
    } as IComputerlog,
  },
  {
    computer: {
      serial: "778899000",
      name: "Asus ROG Zephyrus",
      status: "In Use",
      damage: "Overheating",
      entry_date: "2022-05-01",
    } as IComputer,
    computerlog: {
      id: 10,
      user_id: 10,
      user_name: "Sophia Martinez",
      computer_id: 10,
      return_date: "2023-12-31", // Set to a future date
      entry_date: "2022-05-01",
    } as IComputerlog,
  },
];
