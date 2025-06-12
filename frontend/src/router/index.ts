// Composables
import { createRouter, createWebHashHistory } from "vue-router";

const routes = [
  {
    path: "/",
    component: () => import("@/layouts/default/Default.vue"),
    children: [
      {
        path: "",
        name: "Home",
        // route level code-splitting
        // this generates a separate chunk (Home-[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import("@/views/Home.vue"),
      },
      {
        path: "/accesscard",
        name: "Access Cards",
        component: () => import("@/views/AccessCards.vue"),
      },
      {
        path: "/login",
        name: "Login",
        component: () => import("@/views/LogIn.vue"),
      },
      {
        path: "/users",
        name: "Users",
        component: () => import("@/views/Users.vue"),
      },
      {
        path: "/users/:user_id",
        name: "User",
        component: () => import("@/views/User.vue"),
        props: true,
      },
      {
        path: "/manageroles",
        name: "ManageRoles",
        component: () => import("@/views/ManageRoles.vue"),
      },
      {
        path: "/library",
        name: "Library",
        component: () => import("@/views/Library.vue"),
      },
      {
        path: "/library/admin",
        name: "AdminLibrary",
        component: () => import("@/views/AdminLibrary.vue"),
      },
      {
        path: "/books/:isbn(\\d+)",
        name: "Book",
        component: () => import("@/views/Book.vue"),
      },
      {
        path: "/books",
        name: "Books",
        component: () => import("@/views/Books.vue"),
      },
      {
        path: "/loans/",
        name: "Loans",
        component: () => import("@/views/Loans.vue"),
      },
      {
        path: "/computers",
        name: "Computers",
        component: () => import("@/views/Computers.vue"),
      },
      {
        path: "/computers/:computer_id",
        name: "Computer",
        component: () => import("@/views/Computer.vue"),
      },
      {
        path: "/support",
        name: "Support",
        component: () => import("@/views/Support.vue"),
      },
      {
        path: "/support/:ticket_id/",
        name: "SupportTicket",
        component: () => import("@/views/SupportTicket.vue"),
      },
      {

        path: "/book/:id",
        name: "BookDetailsView",
        component: () => import("@/views/BookDetails.vue"),
        props: true, // Pass the `id` as a prop to the component
      }
    ],
  },
];

const router = createRouter({
  // history: createWebHistory(process.env.BASE_URL), // default setting
  history: createWebHashHistory(),
  routes,
});

export default router;
