import { defineStore } from "pinia";
import { IComputerlog } from "@/types/ComputerTypes";
import { IComputer } from "@/types/ComputerTypes";


interface IUsers {
  id: number;
  name: string;
  email: string;
  klass: string;
  roles: string[];
  computerLogs: IComputerlog[];
}


export const useUserStore = defineStore("UserStore", {
  state: () => {
    return {
      users: [] as IUsers[],
    };
  },
  actions: {
    /*
        Takes image as arg and doesnt return anything. Sends image to backend for processing
        */
   

    /*
        Doesnt take in anything and doesnt return anything. Sends image to backend for processing
        */
  

    async fetchUsers() {
      try {
        const response = await fetch(
          `https://localhost:3001/api/Users`,
          {
            headers: {
              "Content-Type": "application/json",
            },
          },
        );
        if (!response.ok) throw new Error("Failed to get Users");

        this.users.splice(0, this.users.length);

        const data = await response.json();
        console.log(data)
        data.forEach(
          (element: IUsers) => {
            this.users.push({
              id: element.id,
              name: element.name,
              email: element.email,
              klass: element.klass,
              roles: element.roles,
              computerLogs: element.computerLogs


            });

          },
        );
      } catch (error) {
        console.error("Error fetching Users: ", error);
      }
    },

  },
  getters: {
    allUsers(): IUsers[] {
      return this.users;
    },

    userById: (state) => {
        return (id: number): IUsers | undefined => {
            console.log(state.users)
            return state.users.find((user) => user.id === id);
        };
      },
  },
});
