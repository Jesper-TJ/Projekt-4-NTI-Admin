<template>
    <div>
      <!-- The container for the Google Sign-In button -->
      <div ref="googleButton" class="google-button-container"></div>
    </div>
  </template>
  <script lang="ts">
  import { ref, onMounted } from "vue";
  export default {
    name: "GoogleLoginButton",
    props: {
      clientId: {
        type: String,
        required: true, // Client ID must be passed to the component
      },
      onLoginSuccess: {
        type: Function,
        required: true, // Callback for successful login
      },
      onLoginError: {
        type: Function,
        default: () => console.error("Google login failed"), // Optional callback for login error
      },
    },
    setup(props) {
      const googleButton = ref(null); // Reference to the Google Sign-In button container
      const initializeGoogleSignIn = () => {
        if ((window as any).google && props.clientId) {
          (window as any).google.accounts.id.initialize({
            client_id: props.clientId,
            callback: handleCredentialResponse,
          });
          (window as any).google.accounts.id.renderButton(googleButton.value, {
            theme: "outline",
            size: "large",
          });
        } else {
          console.error("Google API script not loaded or Client ID missing");
        }
      };
      const handleCredentialResponse = (response: any) => {
        if (response.credential) {
          props.onLoginSuccess(response.credential); // Trigger success callback
        } else {
          props.onLoginError(); // Trigger error callback
        }
      };
      onMounted(() => {
        initializeGoogleSignIn(); // Initialize Google Sign-In when the component is mounted
      });
      return {
        googleButton, // Expose the ref to the template
      };
    },
  };
  </script>
  <style scoped>
  .google-button-container {
    display: flex;
    justify-content: center;
    align-items: center;
  }
  </style>